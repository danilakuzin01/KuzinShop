namespace KuzinShop.Models.DTO
{
    public class ProductMapper
    {
        public ProductDetailDTO converToProductDetailDTO(ProductModel product)
        {
            ProductDetailDTO productDetailDTO = new ProductDetailDTO();
            productDetailDTO.Publisher = product.Publisher;
            productDetailDTO.Description = product.Description;
            productDetailDTO.Date = product.Date;
            productDetailDTO.CategoryName = product.Category.Name;
            productDetailDTO.Name = product.Name;
            productDetailDTO.Image = product.Image;
            productDetailDTO.Price = product.Price;
            productDetailDTO.Id = product.Id;
            

            foreach (var item in product.ProductAttributes) {
                ProductDetailAttributesDTO attributesDTO = new ProductDetailAttributesDTO();
                attributesDTO.Attribute = item.Attribute;
                if (item.StringValue != null)
                {
                    attributesDTO.Value = item.StringValue;
                }
                else if (item.IntegerValue != null)
                {
                    attributesDTO.Value = item.IntegerValue.ToString();
                }
                else if (item.DateValue != null)
                {
                    attributesDTO.Value = item.DateValue.ToString();
                }
                attributesDTO.Measure = item.Attribute.Measure;
                productDetailDTO.ProductAttributes.Add(attributesDTO);
            }

            return productDetailDTO;
        }

        public CreateProductDTO convertToCreateProductDTO(ProductModel product, Dictionary<string, string> attributes)
        {
            CreateProductDTO createProduct = new CreateProductDTO();

            return createProduct;
        }
    }
}
