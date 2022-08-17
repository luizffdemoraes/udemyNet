using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// hosting

app.MapGet("/", () => "Hello World 3!");
app.MapPost("/", () => new {Name = "Luiz Moraes", Age = 28});
app.MapGet("/AddHeader", (HttpResponse response) => {
    response.Headers.Add("Teste", "Luiz Moraes");
    return new {Name = "Luiz Moraes", Age = 28};
});
//api.app.com/users?datestart={date}&dateend={date}
app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) => {
    return dateStart + " - " + dateEnd;
});
//api.app.com/user/{code}
app.MapGet("/getproduct/{code}", ([FromRoute] string code) => {
    var product = ProductRepository.GetBy(code);
    return product;
});

app.MapPost("/saveproduct", (Product product) => {
    ProductRepository.Add(product);
});

app.MapGet("/getproductbyheader", (HttpRequest request) => {
    return request.Headers["product-code"].ToString();
});

app.MapPut("/editproduct", (Product product) => {
    var productSaved = ProductRepository.GetBy(product.Code);
    productSaved.Name = product.Name;
});

app.MapDelete("/deleteproduct/{code}", ([FromRoute] string code) => {
    var productSaved = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSaved);
});

app.Run();

public static class ProductRepository {
    // Ao passar a classe para static ela passa a sobrevive as requisições
    // gravando na memoria do servidor
    public static List<Product> Products { get; set; }

    public static void Add(Product product) {
        // Conferir se a lista esta vazia
        if(Products == null) 
            // Inicializando a lista
            Products = new List<Product>();
        // Adicionando itens na lista
        Products.Add(product);
    }

    public static Product GetBy(string code) {
        // Expressão lambda de busca verificando igualdade
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    }
}

public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}