using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        private IProductRepository getMockProductRepository()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product { ProductID = 1, Name = "P1" },
                new Product { ProductID = 1, Name = "P2" },
                new Product { ProductID = 1, Name = "P3" },
                new Product { ProductID = 1, Name = "P4" },
                new Product { ProductID = 1, Name = "P5" }
            });

            return mock.Object;
        }

        [TestMethod]
        public void Can_paginate()
        {
            // Arrange
            var fakeProductRepo = getMockProductRepository();
            ProductController controller = new ProductController(fakeProductRepo);
            controller.PageSize = 3;

            // Act
            ProductListViewModel result = controller.List(null, 2).Model as ProductListViewModel;

            // Assert
            if (result is null) Assert.Fail();
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML Helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;

            // Arrange - create Paginginfo data
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Arrange - set up the delegate using lamda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Act
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var fakeProductRepo = getMockProductRepository();
            ProductController controller = new ProductController(fakeProductRepo);
            controller.PageSize = 3;

            // Act
            ProductListViewModel result = controller.List(null, 2).Model as ProductListViewModel;

            // Assert 
            if (result is null) Assert.Fail();
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            // Arrange
            var fakeProductRepo = getMockProductRepository();
            var prodArray = fakeProductRepo.Products.ToArray();
            prodArray[0].Category = "Cat1";
            prodArray[1].Category = "Cat2";
            prodArray[2].Category = "Cat1";
            prodArray[3].Category = "Cat2";
            prodArray[4].Category = "Cat3";

            ProductController controller = new ProductController(fakeProductRepo);
            controller.PageSize = 3;

            // Action
            Product[] result = ((ProductListViewModel)controller.List("Cat2", 1).Model)
                                .Products.ToArray();

            // Assert
            Assert.AreEqual(result.Length, 2);
            Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
    }
}
