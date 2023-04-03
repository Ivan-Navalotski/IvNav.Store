namespace IvNav.Store.Web.Models.V1.Product
{
    /// <summary>
    /// ReadProductsResponseDto.
    /// </summary>
    public class ReadProductsResponseDto
    {
        /// <summary>
        /// Items.
        /// </summary>
        public IReadOnlyCollection<ReadProductResponseDto> Items { get; init; } = null!;
    }
}
