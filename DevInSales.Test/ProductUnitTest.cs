using DevInSales.Context;
using DevInSales.Controllers;
using DevInSales.DTOs;
using DevInSales.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DevInSales.Test;

public class ProductUnitTest
{
    private readonly DbContextOptions<SqlContext> _contextOptions;
    #region Constructor
    public ProductUnitTest()
    {
        _contextOptions = new DbContextOptionsBuilder<SqlContext>()
        .UseInMemoryDatabase("UserControllerUnitTest")
        .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

        using var context = new SqlContext(_contextOptions);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

    }
    #endregion

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public async Task GetProductCompareError()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Test", 200, 150);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task GetProductNameFilter()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Test", null, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Test"));
    }

    [Test]
    public async Task GetProductNameNotFoundFilter()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct("Test", null, null);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task GetProductMinPriceFilter()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, 310, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Test"));
    }

    [Test]
    public async Task GetProductMaxPriceFilter()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, null, 140);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(1));
        Assert.That(content[0].Name.Contains("Test"));
    }

    [Test]
    public async Task GetProductTest()
    {
        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count();

        var controller = new ProductController(context);

        var result = await controller.GetProduct(null, null, null);

        var expected = (result.Result as ObjectResult);

        var content = expected.Value as List<ProductGetDTO>;

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("200"));
        Assert.That(content.Count(), Is.EqualTo(qtdProduct));
    }

    [Test]
    public async Task PostProductProduct()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 13m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PostProductInvalidPrice()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 0m
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PostProductTest()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 13
        };

        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count() + 1;

        var controller = new ProductController(context);

        var result = await controller.PostProduct(product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("201"));
        Assert.That(context.Product.Count(), Is.EqualTo(qtdProduct));
    }

    [Test]
    public async Task PutProductIdNotFound()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 13
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(0, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task PutProductName()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 13
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(2, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PutProductInvalidPrice()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 0
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PutProductNullName()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = null,
            CategoryId = 1,
            Suggested_Price = 10
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PutProductTest()
    {
        var product = new ProductPostAndPutDTO
        {
            Name = "Test",
            CategoryId = 1,
            Suggested_Price = 239
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PutProduct(1, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

   

    [Test]
    public async Task PatchProductNullaName()
    {
        var product = new ProductPatchDTO
        {
            Name = null,
            Suggested_Price = 100,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(5, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
    }

    [Test]
    public async Task PatchProductNameNotFound()
    {
        var product = new ProductPatchDTO
        {
            Name = "Test",
            Suggested_Price = 100,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(2, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task PatchProductIdnotFound()
    {
        var product = new ProductPatchDTO
        {
            Name = "Test",
            Suggested_Price = 239
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(0, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }
    [Test]
    public async Task PatchProductNameAndPriceNull()
    {
        var product = new ProductPatchDTO
        {
            Name = null,
        };

        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.PatchProduct(2, product);

        var expected = (result.Result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

    [Test]
    public async Task DeleteProductTest()
    {
        var context = new SqlContext(_contextOptions);
        var qtdProduct = context.Product.Count() - 1;

        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(10);

        var expected = (result as StatusCodeResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("204"));
        Assert.That(context.Product.Count() == qtdProduct);
    }

    [Test]
    public async Task DeleteProducInvalidId()
    {
        var context = new SqlContext(_contextOptions);

        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(0);

        var expected = (result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("404"));
    }

    [Test]
    public async Task DeleteProductOrderLink()
    {
        var context = new SqlContext(_contextOptions);

        var user = await context.User.FindAsync(1);
        var seller = await context.User.FindAsync(2);
        var order = new Order
        {
            Id = 1,
            User = user,
            UserId = 1,
            Seller = seller,
            SellerId = 2,
            Date_Order = DateTime.Now,
            Shipping_Company = "Sedex",
            Shipping_Company_Price = 22m,
        };
        var product = await context.Product.FindAsync(3);
        var orderProduct = new OrderProduct
        {
            Id = 1,
            Unit_Price = 189,
            Amount = 1,
            Order = order,
            Product = product
        };
        await context.Order_Product.AddAsync(orderProduct);

        await context.SaveChangesAsync();

        var controller = new ProductController(context);

        var result = await controller.DeleteProduct(3);

        var expected = (result as ObjectResult);

        Assert.That(expected.StatusCode.ToString(), Is.EqualTo("400"));
    }

   
}