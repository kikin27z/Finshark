namespace Api.Helper
{
    public class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortingBy{ get; set; } = null;
        public bool Descending { get; set; } = false;

        public int NumPage { get; set; } = 1;
        public int NumSize { get; set; } = 20;
    }
}
