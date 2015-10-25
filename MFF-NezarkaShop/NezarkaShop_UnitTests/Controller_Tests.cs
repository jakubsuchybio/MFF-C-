using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MFF_NezarkaShop;

namespace MFF_NezarkaShop_UnitTests {
    [TestClass]
    public class Controller_Tests {

        private ModelStore ModelMockup() {
            StringReader input = new StringReader(
            "DATA-BEGIN\n" +
            "BOOK;1;Lord of the Rings;J. R. R. Tolkien;59\n" +
            "BOOK;2;Hobbit: There and Back Again;J. R. R. Tolkien;49\n" +
            "BOOK;3;Going Postal;Terry Pratchett;28\n" +
            "BOOK;4;The Colour of Magic;Terry Pratchett;159\n" +
            "BOOK;5;I Shall Wear Midnight;Terry Pratchett;31\n" +
            "CUSTOMER;1;Pavel;Jezek\n" +
            "CUSTOMER;2;Jan;Kofron\n" +
            "CUSTOMER;3;Petr;Hnetynka\n" +
            "CUSTOMER;4;Tomas;Bures\n" +
            "CART-ITEM;2;1;3\n" +
            "CART-ITEM;2;5;1\n" +
            "DATA-END\n");

            return ModelStore.LoadFrom(input);
        }

        private string InvalidPageMockup() {
            StringWriter writer = new StringWriter();
            writer.WriteLine("<!DOCTYPE html>");
            writer.WriteLine("<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">");
            writer.WriteLine("<head>");
            writer.WriteLine("    <meta charset=\"utf-8\" />");
            writer.WriteLine("    <title>Nezarka.net: Online Shopping for Books</title>");
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<p>Invalid request.</p>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.WriteLine("====");

            return writer.ToString();
        }

        #region ProcessCommand Tests
        [TestMethod]
        public void ProcessCommand_IncorrectLength() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model,view);
            try {
                control.ProcessCommand("GET http://www.nezarka.net/Books");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Command length wrong.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_NotGet() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("POST 1 http://www.nezarka.net/Books");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Not a GET request.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_IncorrectDomain() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net1/Books");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Wrong domain.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_CustomerNotExists() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 5 http://www.nezarka.net/Books");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Customer doesn't exists.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPIsTooShort() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("HTTP address is too short.", ex.Message);
            }
        }
        
        [TestMethod]
        public void ProcessCommand_HTTPCmdIsWrong() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/1");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("HTTP address is wrong.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPBooksIsWrong() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/Books/1");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Books HTTP address is wrong.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPBooksDetailNotFound() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/Books/Detail/6");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Book Detail not found.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPBooksDetailIsWrong() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/Books/Detail/1/test");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Books HTTP address is wrong.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPBooksDetailIsWrong2() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 1 http://www.nezarka.net/Books/Detaill/1");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Books HTTP address is wrong.", ex.Message);
            }
        }

        [TestMethod]
        public void ProcessCommand_HTTPShoppingCartIsWrong() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);
            try {
                control.ProcessCommand("GET 4 http://www.nezarka.net/ShoppingCart/1");
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("ShoppingCart HTTP address is wrong.", ex.Message);
            }
        }
        #endregion

        #region AddBook Tests
        [TestMethod]
        public void AddBook_NewBook() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);

            Customer c = model.GetCustomer(2);
            Book b = model.GetBook(2);

            control.AddBook(c, b);

            Assert.AreEqual(model.GetCustomer(2).ShoppingCart.Items.Find(itm => itm.BookId == b.Id).Count, 1);
        }
        
        [TestMethod]
        public void AddBook_IncreaseCount() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);

            Customer c = model.GetCustomer(2);
            Book b = model.GetBook(1);

            control.AddBook(c, b);

            Assert.AreEqual(model.GetCustomer(2).ShoppingCart.Items.Find(itm => itm.BookId == b.Id).Count, 4);
        }
        #endregion

        #region RemoveBook Tests
        [TestMethod]
        public void RemoveBook_NotExistingBook() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);

            Customer c = model.GetCustomer(2);
            Book b = model.GetBook(2);

            try {
                control.RemoveBook(c, b);
                Assert.Fail("Expected exception was not thrown.");
            }
            catch(Exception ex) {
                Assert.AreEqual("Book doesn't exist in Customers ShoppingCart.", ex.Message);
            }
        }
        
        [TestMethod]
        public void RemoveBook_DecreaseCount() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);

            Customer c = model.GetCustomer(2);
            Book b = model.GetBook(1);

            control.RemoveBook(c, b);

            Assert.AreEqual(model.GetCustomer(2).ShoppingCart.Items.Find(itm => itm.BookId == b.Id).Count, 2);
        }

        [TestMethod]
        public void RemoveBook_DeleteBook() {
            StringWriter output = new StringWriter();
            ModelStore model = ModelMockup();
            Viewer view = new Viewer(output);
            Controller control = new Controller(model, view);

            Customer c = model.GetCustomer(2);
            Book b = model.GetBook(5);

            control.RemoveBook(c, b);

            Assert.AreEqual(model.GetCustomer(2).ShoppingCart.Items.Find(itm => itm.BookId == b.Id), null);
        }
        #endregion
    }
}