﻿using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using StockProductTracking.MVVM.Model;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Security;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Forms;

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
                        Id = (int)reader["Id"],
                        created_who = (string)reader["created_who"],
                        created_at = (DateTime)reader["created_at"]
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
                        CategoryId = (int)(long)reader["category_id"],
                        created_who = (string)reader["created_who"],
                        created_at = (DateTime)reader["created_at"]
                    };
                    _CategoryList.Add(category);
                }
                mySqlConnection.Close();
            }
            return _CategoryList;
        }

        public ObservableCollection<Employee> GetEmployee()
        {
            ObservableCollection<Employee> _EmployeeList = new ObservableCollection<Employee>();

            mySqlConnection.Open();
            string query = "select * from employee";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Employee employee = new Employee
                    {
                        Id = (int)reader["employee_id"],
                        FirstName = (string)reader["employee_name"],
                        LastName  = (string)reader["employee_lastname"],
                        Username = (string)reader["employee_username"],
                        Email = (string)reader["employee_mail"],
                        Password = (reader["employee_password"]).ToString(),
                        IsAdmin = (bool)reader["is_admin"],
                        created_who = (string)reader["created_who"],
                        created_at = (DateTime)reader["created_at"]
                    };
                    _EmployeeList.Add(employee);
                }
                mySqlConnection.Close();
            }
            return _EmployeeList;
        }

        public List<string> GetEmployeeUsername()
        {
            List<string> _EmployeeUsername = new List<string>();

            mySqlConnection.Open();
            string query = "select employee_username from employee";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    string Username = (string)reader["employee_username"];
                    _EmployeeUsername.Add(Username);
                }
                mySqlConnection.Close();
            }
            return _EmployeeUsername;
        }
        public List<string> GetEmployeeEmail()
        {
            List<string> _EmployeeEmail = new List<string>();

            mySqlConnection.Open();
            string query = "select employee_mail from employee";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    string mail = (string)reader["employee_mail"];
                    _EmployeeEmail.Add(mail);
                }
                mySqlConnection.Close();
            }
            return _EmployeeEmail;
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
                        OrderProductPrice = (decimal)reader["a_order_product_price"],
                        OrderProductCount = (int)reader["a_order_product_count"],
                        OrderStatus = (bool)reader["a_order_status"]

                    };
                    _OrderList.Add(order);
                }
                mySqlConnection.Close();
            }
            return _OrderList;
        }
        public List<Order> GetAcceptedOrderListByCategories()
        {
            List<Order> orders = new List<Order>();
            mySqlConnection.Open();
            string query = "Select a_orders.a_order_product_price, a_orders.a_order_product_count, products.category_id,category.title " +
                "FROM (a_orders INNER JOIN products ON a_orders.a_order_product_title = products.product_title)" +
                " INNER JOIN category ON products.category_id = category.category_id;";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    Order order = new Order
                    {
                        OrderProductPrice = (decimal)reader["a_order_product_price"],
                        OrderProductCount = (int)reader["a_order_product_count"],
                        OrderCategoryTitle = (string)reader["title"]
                    };
                    orders.Add(order);
                }
                mySqlConnection.Close();
            }
            return orders;
        }
        public string GetTotalPriceOrders()
        {
            string _TotalPriceOrders = "0";

            mySqlConnection.Open();
            string query = "SELECT SUM(a_orders.a_order_product_price * a_orders.a_order_product_count) AS 'sum' FROM a_orders";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {

                    _TotalPriceOrders = Convert.ToString((decimal)reader["sum"]);

                }
                mySqlConnection.Close();
            }
            return _TotalPriceOrders;
        }

        public string GetTotalOrderCount()
        {
            string _TotalOrderCount = "0";

            mySqlConnection.Open();
            string query = "select count(a_order_product_price) as 'count' from a_orders";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {

                    _TotalOrderCount = Convert.ToString((long)reader["count"]);


                }
                mySqlConnection.Close();
            }
            return _TotalOrderCount;
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
                        OrderProductPrice = (decimal)reader["order_product_price"],
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
                        ProductPrice = Convert.ToString(reader["product_price"]),
                        ProductRealPrice = Convert.ToString(reader["product_real_price"]),
                        ProductStock = (int)reader["product_stock"],
                        ProductBrand = (string)reader["product_brand"],
                        created_who = (string)reader["created_who"],
                        created_at = (DateTime)reader["created_at"]
                    };
                    _ProductList.Add(products);
                }
                mySqlConnection.Close();
            }
            return _ProductList;
        }

        public void DeleteCustomer(string id)
        {
            mySqlConnection.Close();
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
            mySqlConnection.Close();
            mySqlConnection.Open();
            string query = "delete from category where category_id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                 command.ExecuteNonQuery();
            }
            mySqlConnection.Close();
           
        }

        public void DeleteEmployee(string id)
        {
            mySqlConnection.Open();
            string query = "delete from employee where employee_id = " + id;
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
            mySqlConnection.Close();
            mySqlConnection.Open();
            string query = "delete from orders where order_id = " + id;
            using (MySqlCommand command = new MySqlCommand(query, mySqlConnection))
            {
                command.ExecuteNonQuery();
            }
            mySqlConnection.Close();
        }

        public void AddCustomer(string _customername, string _lastname, string _phone, string _address,string _created_who)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO customers(name,lastname,phone,address,created_who) VALUES(@name, @lastname,@phone,@address,@created_who)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@name", _customername);
            mySqlCommand.Parameters.AddWithValue("@lastname", _lastname);
            mySqlCommand.Parameters.AddWithValue("@phone", _phone);
            mySqlCommand.Parameters.AddWithValue("@address", _address);
            mySqlCommand.Parameters.AddWithValue("@created_who", _created_who);
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

        public void AddCategory(string _categorytitle,string _created_who)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO category(title,created_who) VALUES(@title,@created_who)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@title", _categorytitle);
            mySqlCommand.Parameters.AddWithValue("@created_who", _created_who);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void AddProduct(int _category_id, string _product_title, int _product_stock, decimal _product_price, decimal _product_real_price, string _product_brand,string _created_who)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO products(category_id,product_title,product_stock,product_price,product_real_price,product_brand,created_who) VALUES(@category_id,@product_title,@product_stock,@product_price,@product_real_price,@product_brand,@created_who)";

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
            mySqlCommand.Parameters.AddWithValue("@created_who", _created_who);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void AddEmployee(string EmployeeFirstName, string EmployeeLastName, string EmployeeUsername, string EmployeePassword, string EmployeeEmail,bool EmployeeIsAdmin,string _created_who)
        {
            mySqlConnection.Open();
            string query = "INSERT INTO employee(employee_name,employee_lastname,employee_username,employee_mail,employee_password,is_admin,created_who) VALUES(@employee_name,@employee_lastname,@employee_username,@employee_mail,@employee_password,@is_admin,@created_who)";

            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.Parameters.AddWithValue("@employee_name", EmployeeFirstName);
            mySqlCommand.Parameters.AddWithValue("@employee_lastname", EmployeeLastName);
            mySqlCommand.Parameters.AddWithValue("@employee_username", EmployeeUsername);
            mySqlCommand.Parameters.AddWithValue("@employee_mail", EmployeeEmail);
            mySqlCommand.Parameters.AddWithValue("@employee_password", EmployeePassword);
            mySqlCommand.Parameters.AddWithValue("@is_admin", EmployeeIsAdmin);
            mySqlCommand.Parameters.AddWithValue("@created_who", _created_who);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateCustomer(int id, string _customername, string _lastname, string _phone, string _address, string _updated_who)
        {
            mySqlConnection.Open();
            string query = $"UPDATE customers SET name='{_customername}', lastname='{_lastname}',phone='{_phone}' , address='{_address}', created_at='{DateTime.Now.ToString("u")}', created_who='{_updated_who}' WHERE Id ={id}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateCategory(int id, string _categorytitle,string _updated_who)
        {
            mySqlConnection.Open();
            string query = $"UPDATE category SET title='{_categorytitle}', created_at='{DateTime.Now.ToString("u")}', created_who='{_updated_who}' WHERE category_id ={id}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void UpdateProduct(int id, int _category_id, string _product_title, int _product_stock, decimal _product_price, decimal _product_real_price, string _product_brand,string _created_who)
        {
            mySqlConnection.Open();
            string query = $"UPDATE products SET category_id=@category_id, product_title=@product_title ,product_stock=@product_stock, product_price=@product_price, product_real_price=@product_real_price ,product_brand=@product_brand ,created_at='{DateTime.Now.ToString("u")}',created_who='{_created_who}' WHERE Id ={id}";

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

        public void UpdateEmployee(int EmployeeId ,string EmployeeFirstName, string EmployeeLastName, string EmployeeUsername, string EmployeePassword, string EmployeeEmail, bool EmployeeIsAdmin , string _updated_who)
        {
            mySqlConnection.Open();
            string query = $"UPDATE employee SET employee_name='{EmployeeFirstName}', employee_lastname='{EmployeeLastName}' ,employee_username='{EmployeeUsername}', employee_password='{EmployeePassword}', employee_mail='{EmployeeEmail}' ,is_admin={EmployeeIsAdmin} ,created_who='{_updated_who}' , created_at='{DateTime.Now.ToString("u")}' WHERE employee_id ={EmployeeId}";
            mySqlCommand = new MySqlCommand(query, mySqlConnection)
            {
                CommandText = query
            };
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public void SetStatusToAccepted(int orderId)
        {

            mySqlConnection.Close();
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

        public Dictionary<string,decimal> GetPieChartKeyValueFromDatabaseWithCategory(string categoryTitle)
        {
            Dictionary<string, decimal> _PieChartKeyValue = new Dictionary<string, decimal>();
            string query = "SELECT a_orders.a_order_product_title as productTitle, Sum(a_orders.a_order_product_count) as productSum " +
                "FROM ((products CROSS join category ON products.category_id = category.category_id) " +
                "CROSS JOIN a_orders ON product_title = a_orders.a_order_product_title) " +
                "WHERE category.title = @title " +
                "GROUP BY a_orders.a_order_product_title;";
            
            mySqlConnection.Open();
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlCommand.Parameters.AddWithValue("@title", categoryTitle);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    _PieChartKeyValue.Add((string)reader["productTitle"], Convert.ToDecimal(reader["productSum"]));

                }
                mySqlConnection.Close();
            }
            return _PieChartKeyValue;
        }

        public Dictionary<string, decimal> GetPieChartKeyValueFromDatabase()
        {

            Dictionary<string, decimal> _PieChartKeyValue = new Dictionary<string, decimal>();

            string _UniqueCategoryNames;
            decimal _TotalCount;

            mySqlConnection.Open();
            string query = "SELECT DISTINCT(category.title) as 'unique_category_name', sum(a_orders.a_order_product_count) as 'sum_total_order_groupbycategory' FROM category INNER JOIN products ON products.category_id = category.category_id INNER JOIN a_orders ON a_orders.a_order_product_title = products.product_title GROUP BY category.title";

            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {

                while (reader.Read())
                {
                    _UniqueCategoryNames = (string)reader["unique_category_name"];
                    _TotalCount = (decimal)reader["sum_total_order_groupbycategory"];
                    _PieChartKeyValue.Add(_UniqueCategoryNames, _TotalCount);

                }
                mySqlConnection.Close();
            }
            return _PieChartKeyValue;
        }

        public Employee LogIn(NetworkCredential credential)
        {
            mySqlConnection.Open();
            string query = $"select * from employee where binary employee_username = \'{credential.UserName}\' and employee_password=\'{credential.Password}\'";

            Employee employee = null;
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    employee = new Employee
                    {
                        Id = (int)reader["employee_id"],
                        FirstName = (string)reader["employee_name"],
                        LastName = (string)reader["employee_lastname"],
                        Username = (string)reader["employee_username"],
                        Email = (string)reader["employee_mail"],
                        IsAdmin = (bool)reader["is_admin"]
                    };
                }
                mySqlConnection.Close();
            }
            return employee;
        }
        public void ChangePasswordFromEmail(string _pass ,string _email)
        {
            mySqlConnection.Open();
            string query = $"UPDATE employee SET employee_password=@password WHERE employee_mail =@email";
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlCommand.Parameters.AddWithValue("@email", _email);
            mySqlCommand.Parameters.AddWithValue("@password", _pass);
            mySqlCommand.ExecuteNonQuery();
            mySqlConnection.Close();

        }

        public Employee GetByUsername(string username)
        {
            mySqlConnection.Open();
            string query = $"select * from employee where employee_username = @username";

            Employee employee = null;
            mySqlCommand = new MySqlCommand(query, mySqlConnection);
            mySqlCommand.Parameters.AddWithValue("@username", username);
            using (MySqlDataReader reader = mySqlCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    employee = new Employee
                    {
                        Id = (int)reader["employee_id"],
                        FirstName = (string)reader["employee_name"],
                        LastName = (string)reader["employee_lastname"],
                        Username = (string)reader["employee_username"],
                        Email = (string)reader["employee_mail"],
                        IsAdmin = (bool)reader["is_admin"]
                    };
                }
                mySqlConnection.Close();
            }
            return employee;
        }
    }
}
