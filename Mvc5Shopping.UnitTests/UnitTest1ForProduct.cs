using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mvc5Shopping.Domain.Abstract;
using Mvc5Shopping.Domain.Entities;
using Mvc5Shopping.WebUI.Controllers;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Mvc5Shopping.WebUI.Models;
using Mvc5Shopping.WebUI.HtmlHelpers;

namespace Mvc5Shopping.UnitTests
{
    [TestClass]
    public class UnitTest1ForProduct
    {
        //[TestMethod]
        //public void Can_Paginate()
        //{
        //    // Arrange
        //    Mock<IProductRepository> mock = new Mock<IProductRepository>();
        //    mock.Setup(m => m.Products).Returns(new Product[]
        //    {
        //        new Product { ProductID = 1, Name = "p1" },
        //        new Product { ProductID = 2, Name = "p2" },
        //        new Product { ProductID = 3, Name = "p3" },
        //        new Product { ProductID = 4, Name = "p4" },
        //        new Product { ProductID = 5, Name = "p5" },
        //    });

        //    ProductController controller = new ProductController(mock.Object);
        //    controller.PageSize = 3;

        //    // Act
        //    IEnumerable<Product> result = (IEnumerable<Product>)controller.List(2).Model;

        //    // Assert
        //    Product[] prodArray = result.ToArray();
        //    Assert.IsTrue(prodArray.Length == 2);
        //    Assert.AreEqual(prodArray[0].Name, "p4");
        //    Assert.AreEqual(prodArray[1].Name, "p5");
        //}

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrage - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;

            // Arrange - create PagingInfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1" },
                new Product {ProductID = 2, Name = "p2" },
                new Product {ProductID = 3, Name = "p3" },
                new Product {ProductID = 4, Name = "p4" },
                new Product {ProductID = 5, Name = "p5" },
            });

            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            // Assert 
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Paginate2()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1" },
                new Product {ProductID = 2, Name = "p2" },
                new Product {ProductID = 3, Name = "p3" },
                new Product {ProductID = 4, Name = "p4" },
                new Product {ProductID = 5, Name = "p5" },
            });

            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;

            // Assert 
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "p4");
            Assert.AreEqual(prodArray[1].Name, "p5");
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1", Category = "Cat1" },
                new Product {ProductID = 2, Name = "p2", Category = "Cat2" },
                new Product {ProductID = 3, Name = "p3", Category = "Cat1" },
                new Product {ProductID = 4, Name = "p4", Category = "Cat2" },
                new Product {ProductID = 5, Name = "p5", Category = "Cat3" },
            });

            // Arrange
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            // Act
            //ProductsListViewModel result = (ProductsListViewModel)controller.List("Cat2", 2).Model;
            Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

            // Assert 
            //Product[] prodArray = result.Products.ToArray();
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "p2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "p4" && result[1].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1", Category = "Apples" },
                new Product {ProductID = 2, Name = "p2", Category = "Apples" },
                new Product {ProductID = 3, Name = "p3", Category = "Plums" },
                new Product {ProductID = 4, Name = "p4", Category = "Oranges" }                
            });

            // Arrange - create the controller
            NavController target = new NavController(mock.Object);
            
            // Act = get the set of categories            
            string[] result = ((IEnumerable<string>)target.Menu().Model).ToArray();

            // Assert            
            Assert.AreEqual(result.Length, 3);
            Assert.AreEqual(result[0], "Apples");
            Assert.AreEqual(result[1], "Oranges");
            Assert.AreEqual(result[2], "Plums");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {ProductID = 1, Name = "p1", Category = "Apples" },
                new Product {ProductID = 4, Name = "p2", Category = "Oranges" },
            });

            // Arrange - create the controller
            NavController target = new NavController(mock.Object);

            // Arrange - define the category to selected
            string categoryToSelect = "Apples";

            // Action
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Assert
            Assert.AreEqual(categoryToSelect, result);
        }

        [TestMethod]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange - create the mock repository
            Mock<IProductRepository> mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            });

            // Arrange - create a controller and make the page size 3 items
            ProductController target = new ProductController(mock.Object);
            target.PageSize = 3;

            // Action - test the product counts for different categories
            int res1 = ((ProductsListViewModel)target.List("Cat1").Model).PagingInfo.TotalItems;
            int res2 = ((ProductsListViewModel)target.List("Cat2").Model).PagingInfo.TotalItems;
            int res3 = ((ProductsListViewModel)target.List("Cat3").Model).PagingInfo.TotalItems;
            int resAll = ((ProductsListViewModel)target.List(null).Model).PagingInfo.TotalItems;

            // Assert
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }        
    }
}
