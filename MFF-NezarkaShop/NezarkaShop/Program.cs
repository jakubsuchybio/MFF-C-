using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("NezarkaShop_UnitTests")]

namespace MFF_NezarkaShop {

    #region Model from CodEx (untouched)
    class ModelStore {
        private List<Book> books = new List<Book>();
        private List<Customer> customers = new List<Customer>();

        public IList<Book> GetBooks() {
            return books;
        }

        public Book GetBook(int id) {
            return books.Find(b => b.Id == id);
        }

        public Customer GetCustomer(int id) {
            return customers.Find(c => c.Id == id);
        }

        public static ModelStore LoadFrom(TextReader reader) {
            var store = new ModelStore();

            try {
                if(reader.ReadLine() != "DATA-BEGIN") {
                    return null;
                }
                while(true) {
                    string line = reader.ReadLine();
                    if(line == null) {
                        return null;
                    }
                    else if(line == "DATA-END") {
                        break;
                    }

                    string[] tokens = line.Split(';');
                    switch(tokens[0]) {
                        case "BOOK":
                            store.books.Add(new Book {
                                Id = int.Parse(tokens[1]),
                                Title = tokens[2],
                                Author = tokens[3],
                                Price = decimal.Parse(tokens[4])
                            });
                            break;
                        case "CUSTOMER":
                            store.customers.Add(new Customer {
                                Id = int.Parse(tokens[1]),
                                FirstName = tokens[2],
                                LastName = tokens[3]
                            });
                            break;
                        case "CART-ITEM":
                            var customer = store.GetCustomer(int.Parse(tokens[1]));
                            if(customer == null) {
                                return null;
                            }
                            customer.ShoppingCart.Items.Add(new ShoppingCartItem {
                                BookId = int.Parse(tokens[2]),
                                Count = int.Parse(tokens[3])
                            });
                            break;
                        default:
                            return null;
                    }
                }
            }
            catch(Exception ex) {
                if(ex is FormatException || ex is IndexOutOfRangeException) {
                    return null;
                }
                throw;
            }

            return store;
        }
    }

    class Book {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
    }

    class Customer {
        private ShoppingCart shoppingCart;

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ShoppingCart ShoppingCart {
            get {
                if(shoppingCart == null) {
                    shoppingCart = new ShoppingCart();
                }
                return shoppingCart;
            }
            set {
                shoppingCart = value;
            }
        }
    }

    class ShoppingCartItem {
        public int BookId { get; set; }
        public int Count { get; set; }
    }

    class ShoppingCart {
        public int CustomerId { get; set; }
        public List<ShoppingCartItem> Items = new List<ShoppingCartItem>();
    }
    #endregion

    /// <summary> Controller is for processing requests, changing model and showing views. </summary>
    class Controller {
        private const string domain = "http://www.nezarka.net/";
        private ModelStore model;
        private Viewer view;

        public Controller(ModelStore model, Viewer view) {
            this.model = model;
            this.view = view;
        }

        /// <summary> Processes all lines from stdIn. Shows InvalidPage on invalid command. </summary>
        public void ProcessAllCommands(TextReader stdIn) {
            while(true) {
                try {
                    string line = stdIn.ReadLine();
                    if(line == null) {
                        return;
                    }

                    ProcessCommand(line);
                }
                catch(Exception) {
                    view.ShowInvalidRequest();
                }
            }
        }

        /// <summary> Parses one command and makes action or shows view. </summary>
        /// <param name="line">One command as string.</param>
        /// <exception cref="System.Exception">Throws when command is not correct by definition.</exception>
        internal void ProcessCommand(string line) {
            string[] tokens = line.Split(' ');
            if(tokens.Length != 3)
                throw new Exception("Command length wrong.");
                        
            if(tokens[0] != "GET")
                throw new Exception("Not a GET request.");
                                        
            if(tokens[2].Substring(0, domain.Length) != domain)
                throw new Exception("Wrong domain.");
                    
            Customer customer;
            if((customer = model.GetCustomer(int.Parse(tokens[1]))) == null)
                throw new Exception("Customer doesn't exists.");


            string[] cmds = tokens[2].
                Substring(domain.Length).
                Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            if(cmds.Length == 0)
                throw new Exception("HTTP address is too short.");

            switch(cmds[0]) {
                case "Books":
                    if(cmds.Length == 1)
                        view.ShowBooks(model.GetBooks(), customer);
                    else if(cmds.Length == 3 && cmds[1] == "Detail") {
                        Book detail = model.GetBook(int.Parse(cmds[2]));
                        if(detail == null)
                            throw new Exception("Book Detail not found.");
                        view.ShowBookDetail(detail, customer);
                    }
                    else
                        throw new Exception("Books HTTP address is wrong.");
                    break;
                case "ShoppingCart":
                    if(cmds.Length == 1)
                        view.ShowShoppingCart(customer, model);
                    else if(cmds.Length == 3)
                        switch(cmds[1]) {
                            case "Add":
                                AddBook(customer, model.GetBook(int.Parse(cmds[2])));
                                break;
                            case "Remove":
                                RemoveBook(customer, model.GetBook(int.Parse(cmds[2])));
                                break;
                            default:
                                throw new Exception("ShoppingCart HTTP address is wrong.");
                        }
                    else
                        throw new Exception("ShoppingCart HTTP address is wrong.");
                    break;
                default:
                    throw new Exception("HTTP address is wrong.");
            }
        }

        /// <summary> Adds book to Customer's ShoppingCart or increases book's count by 1.</summary>
        /// <param name="customer">Customer object for custId from command.</param>
        /// <param name="book">Book object for BookId from command.</param>
        /// <exception cref="System.Exception">Throws when customer or book doesn't exist.</exception>
        internal void AddBook(Customer customer, Book book) {
            if(customer == null || book == null)
                throw new Exception("Customer or Book doesn't exist.");

            ShoppingCart cart = customer.ShoppingCart;
            ShoppingCartItem cItem = cart.Items.Find(itm => itm.BookId == book.Id);
            if(cItem != null)
                cItem.Count++;
            else
                cart.Items.Add(new ShoppingCartItem {
                    BookId = book.Id,
                    Count = 1
                });

            view.ShowShoppingCart(customer, model);
        }

        /// <summary> Removes book from Customer's ShoppingCart or decreases book's count by 1. </summary>
        /// <param name="customer">Customer object for custId from command.</param>
        /// <param name="book">Book object for BookId from command.</param>
        /// <exception cref="System.Exception">Throws when Book isn't in Customer's ShoppingCart.</exception>
        /// <exception cref="System.Exception">
        ///     Throws when Book isn't in Customer's ShoppingCart or when customer or book doesn't exist.
        /// </exception>
        internal void RemoveBook(Customer customer, Book book) {
            if(customer == null || book == null)
                throw new Exception("Customer or Book doesn't exist.");

            ShoppingCart cart = customer.ShoppingCart;
            ShoppingCartItem cItem = cart.Items.Find(itm => itm.BookId == book.Id);
            if(cItem == null)
                throw new Exception("Book doesn't exist in Customers ShoppingCart.");

            if(cItem.Count > 1)
                cItem.Count--;
            else
                cart.Items.Remove(cItem);

            view.ShowShoppingCart(customer, model);
        }
    }

    /// <summary> Viewer is an 'interface' for showing views. </summary>
    class Viewer {
        TextWriter stdOut;

        public Viewer(TextWriter stdOut) {
            this.stdOut = stdOut;
        }

        /// <summary> Shows HTML source for list of books. (In table where there are max 3 books in row) </summary>
        /// <param name="items">List of books from model.</param>
        /// <param name="customer">Customer object.</param>
        public void ShowBooks(IList<Book> items, Customer customer) {
            PrintHeadAndMenu(customer.FirstName, customer.ShoppingCart.Items.Count);

            stdOut.WriteLine("    Our books for you:");
            stdOut.WriteLine("    <table>");

            var index = 0;
            var booksLeft = items.Count;
            while(booksLeft > 0) {
                stdOut.WriteLine("        <tr>");
                var columns = Math.Min(3, booksLeft); //This calculates how much items will be displayed in next row
                for(int i = 0; i < columns; i++) {
                    var book = items[index++];
                    stdOut.WriteLine("            <td style=\"padding: 10px;\">");
                    stdOut.WriteLine("                <a href=\"/Books/Detail/" + book.Id + "\">" + book.Title + "</a><br />");
                    stdOut.WriteLine("                Author: " + book.Author + "<br />");
                    stdOut.WriteLine("                Price: " + book.Price + " EUR &lt;<a href=\"/ShoppingCart/Add/" + book.Id + "\">Buy</a>&gt;");
                    stdOut.WriteLine("            </td>");
                }
                booksLeft -= columns;
                stdOut.WriteLine("        </tr>");
            }

            stdOut.WriteLine("    </table>");
            stdOut.WriteLine("</body>");
            stdOut.WriteLine("</html>");

            PrintCmdEnd();
        }

        /// <summary> Shows HTML source for one book detail. </summary>
        /// <param name="item">Book object from model.</param>
        /// <param name="customer">Customer object.</param>
        public void ShowBookDetail(Book item, Customer customer) {
            PrintHeadAndMenu(customer.FirstName, customer.ShoppingCart.Items.Count);

            stdOut.WriteLine("    Book details:");
            stdOut.WriteLine("    <h2>" + item.Title + "</h2>");
            stdOut.WriteLine("    <p style=\"margin-left: 20px\">");
            stdOut.WriteLine("    Author: " + item.Author + "<br />");
            stdOut.WriteLine("    Price: " + item.Price + " EUR<br />");
            stdOut.WriteLine("    </p>");
            stdOut.WriteLine("    <h3>&lt;<a href=\"/ShoppingCart/Add/" + item.Id + "\">Buy this book</a>&gt;</h3>");
            stdOut.WriteLine("</body>");
            stdOut.WriteLine("</html>");
            
            PrintCmdEnd();
        }

        /// <summary> Shows HTML source for Shopping Cart of one customer. (In table with price calculation) </summary>
        /// <param name="customer">Customer object.</param>
        /// <param name="model">Model object. (For getting book details) </param>
        public void ShowShoppingCart(Customer customer, ModelStore model) {
            var cartCount = customer.ShoppingCart.Items.Count;
            PrintHeadAndMenu(customer.FirstName, cartCount);
            
            if(cartCount == 0)  // Special output case when cart is empty
                stdOut.WriteLine("    Your shopping cart is EMPTY.");
            else {
                stdOut.WriteLine("    Your shopping cart:");
                stdOut.WriteLine("    <table>");
                stdOut.WriteLine("        <tr>");
                stdOut.WriteLine("            <th>Title</th>");
                stdOut.WriteLine("            <th>Count</th>");
                stdOut.WriteLine("            <th>Price</th>");
                stdOut.WriteLine("            <th>Actions</th>");
                stdOut.WriteLine("        </tr>");
                decimal totalPrice = 0;
                foreach(var item in customer.ShoppingCart.Items) {
                    var book = model.GetBook(item.BookId);
                    var bookTotalPrice = item.Count * book.Price;
                    totalPrice += bookTotalPrice;
                    
                    stdOut.WriteLine("        <tr>");
                    stdOut.WriteLine("            <td><a href=\"/Books/Detail/" + item.BookId + "\">" + book.Title + "</a></td>");
                    stdOut.WriteLine("            <td>" + item.Count + "</td>");
                    if(item.Count == 1)
                        stdOut.WriteLine("            <td>" + bookTotalPrice + " EUR</td>");
                    else
                        stdOut.WriteLine("            <td>" + item.Count + " * " + book.Price + " = " + bookTotalPrice + " EUR</td>");
                    stdOut.WriteLine("            <td>&lt;<a href=\"/ShoppingCart/Remove/" + item.BookId + "\">Remove</a>&gt;</td>");
                    stdOut.WriteLine("        </tr>");                    
                }
                stdOut.WriteLine("    </table>");
                stdOut.WriteLine("    Total price of all items: " + totalPrice + " EUR");
            }


            stdOut.WriteLine("</body>");
            stdOut.WriteLine("</html>");
            
            PrintCmdEnd();
        }

        /// <summary> Shows HTML source for Invalid request. (When command is not correct) </summary>
        public void ShowInvalidRequest() {
            PrintHead();

            stdOut.WriteLine("<body>");
            stdOut.WriteLine("<p>Invalid request.</p>");
            stdOut.WriteLine("</body>");
            stdOut.WriteLine("</html>");

            PrintCmdEnd();
        }

        /// <summary> Helper function for head of HTML source. </summary>
        private void PrintHead() {
            stdOut.WriteLine("<!DOCTYPE html>");
            stdOut.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            stdOut.WriteLine("<head>");
            stdOut.WriteLine("    <meta charset=\"utf-8\" />");
            stdOut.WriteLine("    <title>Nezarka.net: Online Shopping for Books</title>");
            stdOut.WriteLine("</head>");
        }

        /// <summary> Helper function for head and menu of HTML source. </summary>
        private void PrintHeadAndMenu(string name, int cartCount) {
            PrintHead();
            stdOut.WriteLine("<body>");
            stdOut.WriteLine("    <style type=\"text/css\">");
            stdOut.WriteLine("        table, th, td {");
            stdOut.WriteLine("            border: 1px solid black;");
            stdOut.WriteLine("            border-collapse: collapse;");
            stdOut.WriteLine("        }");
            stdOut.WriteLine("        table {");
            stdOut.WriteLine("            margin-bottom: 10px;");
            stdOut.WriteLine("        }");
            stdOut.WriteLine("        pre {");
            stdOut.WriteLine("            line-height: 70%;");
            stdOut.WriteLine("        }");
            stdOut.WriteLine("    </style>");
            stdOut.WriteLine("    <h1><pre>  v,<br />Nezarka.NET: Online Shopping for Books</pre></h1>");
            stdOut.WriteLine("    " + name + ", here is your menu:");
            stdOut.WriteLine("    <table>");
            stdOut.WriteLine("        <tr>");
            stdOut.WriteLine("            <td><a href=\"/Books\">Books</a></td>");
            stdOut.WriteLine("            <td><a href=\"/ShoppingCart\">Cart (" + cartCount + ")</a></td>");
            stdOut.WriteLine("        </tr>");
            stdOut.WriteLine("    </table>");
        }

        /// <summary> Helper function for spacer between commands. </summary>
        private void PrintCmdEnd() {
            stdOut.WriteLine("====");
        }
    }

    class Program {
        /// <summary> Main function which handles Loading from input and processing to output </summary>
        /// <param name="args">Program arguments.</param>
        /// <param name="stdIn">Input stream object.</param>
        /// <param name="stdOut">Output stream object.</param>
        public static void Run(string[] args, TextReader stdIn, TextWriter stdOut) {
            ModelStore store = ModelStore.LoadFrom(stdIn);

            if(store == null) {
                stdOut.Write("Data error.");
                return;
            }

            Viewer view = new Viewer(stdOut);
            Controller controller = new Controller(store, view);
            controller.ProcessAllCommands(stdIn);
        }

        static void Main(string[] args) {
            Run(args, Console.In, Console.Out);
        }
    }
}