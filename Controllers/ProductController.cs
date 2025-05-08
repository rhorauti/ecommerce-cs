using System.Text.Json;

using e_commerce_cs.aws;
using e_commerce_cs.Models;
using e_commerce_cs.Repositories;

using Microsoft.AspNetCore.Mvc;

namespace e_commerce_cs.Controllers
{
  [ApiController]
  public class Home(S3Connector s3Connector, ProductRepository productRepository) : ControllerBase
  {
    [HttpGet(ENDPOINT.BASE + "products")]
    public async Task<IActionResult> GetProducts()
    {
      List<string> objects = await s3Connector.ListPreSignedUrlsInFolderAsync("rhorauti-products-photo", "products/");
      Console.WriteLine("objects", JsonSerializer.Serialize(objects));
      List<Product> products = await productRepository.GetProductAsync();
      Console.WriteLine("producs: " + JsonSerializer.Serialize(products));
      return Ok(ApiResponse<List<Product>>.Ok("Dados recebidos com sucesso.", products));
    }
  }
}
