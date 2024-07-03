using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace ProductsMgtUI
{
   

    public partial class Form1 : Form
    {
        string connectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;

        private readonly DBService _dbService;


        private DBService dbService;
        public Form1()
        {
            InitializeComponent();

            var serviceProvider = new ServiceCollection()
            .AddDbContext<AppDbContext>(options =>
                options.UseMySql(connectionString,
                    new MySqlServerVersion(new Version(8, 0, 21))))
            .BuildServiceProvider();

            var context = serviceProvider.GetRequiredService<AppDbContext>();
            _dbService = new DBService(context);


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var newProduct = new Product { Name = "New Product", Price = 99.99m, Stocks = 100 };
            //_dbService.AddProduct(newProduct);
            //MessageBox.Show("New Product Added!");
            this.LoadProducts();
        }

        private void LoadProducts()
        {
            var products = _dbService.GetAllProducts();
            
            dgProducts.DataSource = products.Where(x => x.Id == 1).ToList();
        }
    }
}
