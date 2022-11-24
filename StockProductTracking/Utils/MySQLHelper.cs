﻿using MySql.Data.MySqlClient;
using StockProductTracking.MVVM.Model;
using System.Collections.ObjectModel;

namespace StockProductTracking.Utils
{
    class Connect
    {
        private const string SERVERNAME = "localhost";
        private const string USERNAME = "root";
        private const string DATABASE = "stockmanagement";

        private readonly MySqlConnection mySqlConnection;
        private MySqlCommand mySqlCommand;

        private readonly string CONNECTION_STRING;

        public Connect()
        {
            CONNECTION_STRING = $"server = {SERVERNAME}; uid={USERNAME};database={DATABASE}";
            mySqlConnection = new MySqlConnection();
            mySqlCommand = new MySqlCommand();
            mySqlConnection.ConnectionString = CONNECTION_STRING;
        }

        public ObservableCollection<Customer> GetCustomers()
        {
            ObservableCollection<Customer> _CustomersList = new ObservableCollection<Customer>();

            mySqlConnection.Open();
            string query = "select * from customers";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    Customer customer = new Customer
                    {
                        Address = (string)reader["Address"],
                        Phone = (string)reader["Phone"],
                        LastName = (string)reader["LastName"],
                        Name = (string)reader["Name"],
                        Id = (int)reader["Id"]
                    };
                    _CustomersList.Add(customer);
                }
                mySqlConnection.Close();
            }
            return _CustomersList;
        }

        public ObservableCollection<Category> GetCategory()
        {
            ObservableCollection<Category> _CategoryList = new ObservableCollection<Category>();

            mySqlConnection.Open();
            string query = "select * from category";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Category category = new Category
                    {
                        CategoryTitle = (string)reader["title"],
                        CategoryId = (int)(long)reader["category_id"]
                    };
                    _CategoryList.Add(category);
                }
                mySqlConnection.Close();
            }
            return _CategoryList;
        }

        public ObservableCollection<Order> GetAcceptedOrders()
        {
            ObservableCollection<Order> _OrderList = new ObservableCollection<Order>();

            mySqlConnection.Open();
            string query = "select * from a_orders";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Order order = new Order
                    {
                        OrderId = (int)reader["a_order_id"],
                        OrderProductTitle = (string)reader["a_order_product_title"],
                        CustomerId = (int)reader["a_order_customer_id"],
                        OrderProductPrice = (int)reader["a_order_product_price"],
                        OrderProductCount = (int)reader["a_order_product_count"],
                        OrderStatus = (bool)reader["a_order_status"]
                    };
                    _OrderList.Add(order);
                }
                mySqlConnection.Close();
            }
            return _OrderList;
        }

        public ObservableCollection<Order> GetOrders()
        {
            ObservableCollection<Order> _OrderList = new ObservableCollection<Order>();

            mySqlConnection.Open();
            string query = "select * from orders";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Order order = new Order
                    {
                        OrderId = (int)reader["order_id"],
                        OrderProductTitle = (string)reader["order_product_title"],
                        CustomerId = (int)reader["order_customer_id"],
                        OrderProductPrice = (int)reader["order_product_price"],
                        OrderProductCount = (int)reader["order_product_count"],
                        OrderStatus = (bool)reader["order_status"]
                    };
                    _OrderList.Add(order);
                }
                mySqlConnection.Close();
            }
            return _OrderList;
        }

        public ObservableCollection<Product> GetProducts()
        {
            ObservableCollection<Product> _ProductList = new ObservableCollection<Product>();

            mySqlConnection.Open();
            string query = "select * from products";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Product products = new Product
                    {
                        ProductId = (int)(long)reader["id"],
                        CategoryId = (int)(long)reader["category_id"],
                        ProductTitle = (string)reader["product_title"],
                        ProductPrice = (int)reader["product_price"],
                        ProductRealPrice = (int)reader["product_real_price"],
                        ProductStock = (int)reader["product_stock"],
                        ProductBrand = (string)reader["product_brand"]
                    };
                    _ProductList.Add(products);
                }
                mySqlConnection.Close();
            }
            return _ProductList;
        }

        public void DeleteCustomer(string id)
        {
            mySqlConnection.Open();
            string query = "delete from customers where Id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                command.ExecuteNonQuery();
            }

            mySqlConnection.Close();
        }


        public void DeleteCategory(string id)
        {
            mySqlConnection.Open();
            string query = "delete from category where category_id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                command.ExecuteNonQuery();
            }

            mySqlConnection.Close();

        }


        public void DeleteProduct(string id)
        {
            mySqlConnection.Open();
            string query = "delete from products where id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                command.ExecuteNonQuery();
            }
            mySqlConnection.Close();
        }

        public void DeleteOrder(string id)
        {
            mySqlConnection.Open();
            string query = "delete from orders where order_id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                command.ExecuteNonQuery();
            }
            mySqlConnection.Close();
        }

        public void AddCustomer(string _customername, string _lastname, string _phone, string _address)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO customers(name,lastname,phone,address) VALUES(@name, @lastname,@phone,@address)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@name", _customername);
            mySqlCommand.Parameters.AddWithValue("@lastname", _lastname);
            mySqlCommand.Parameters.AddWithValue("@phone", _phone);
            mySqlCommand.Parameters.AddWithValue("@address", _address);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void AddOrder(int _customer_id, string _order_product_title, int _order_product_count, bool _order_status)
        {
            mySqlConnection.Open();
            string query = $"INSERT INTO orders(order_customer_id,order_product_title,order_product_price,order_product_count,order_status) VALUES(@customer_id, @title,(select product_price from products where product_title='{_order_product_title}'),@count,@status)";


            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@customer_id", _customer_id);
            mySqlCommand.Parameters.AddWithValue("@title", _order_product_title);
            mySqlCommand.Parameters.AddWithValue("@count", _order_product_count);
            mySqlCommand.Parameters.AddWithValue("@status", _order_status);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void AddCategory(string _categorytitle)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO category(title) VALUES(@title)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@title", _categorytitle);

            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void AddProduct(int _category_id, string _product_title, int _product_stock, int _product_price, int _product_real_price, string _product_brand)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO products(category_id,product_title,product_stock,product_price,product_real_price,product_brand) VALUES(@category_id,@product_title,@product_stock,@product_price,@product_real_price,@product_brand)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@category_id", _category_id);
            mySqlCommand.Parameters.AddWithValue("@product_title", _product_title);
            mySqlCommand.Parameters.AddWithValue("@product_stock", _product_stock);
            mySqlCommand.Parameters.AddWithValue("@product_brand", _product_brand);
            mySqlCommand.Parameters.AddWithValue("@product_price", _product_price);
            mySqlCommand.Parameters.AddWithValue("@product_real_price", _product_real_price);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateCustomer(int id, string _customername, string _lastname, string _phone, string _address)
        {
            mySqlConnection.Open();
            string query = $"UPDATE customers SET name='{_customername}', lastname='{_lastname}',phone='{_phone}' , address='{_address}' WHERE Id ={id}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateCategory(int id, string _categorytitle)
        {
            mySqlConnection.Open();
            string query = $"UPDATE category SET name='{_categorytitle}' WHERE Id ={id}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateProduct(int id, int _category_id, string _product_title, int _product_stock, int _product_price, int _product_real_price, string _product_brand)
        {
            mySqlConnection.Open();
            string query = $"UPDATE products SET category_id={_category_id}, product_title='{_product_title}' ,product_stock={_product_stock}, product_price={_product_price}, product_real_price={_product_real_price} ,product_brand='{_product_brand}'  WHERE Id ={id}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void SetStatusToAccepted(int orderId)
        {

            mySqlConnection.Open();
            string query = $"UPDATE orders SET order_status = true WHERE order_id = {orderId}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();



            string query2 = $"DELETE FROM orders WHERE order_id = {orderId}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query2
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

    }
}
