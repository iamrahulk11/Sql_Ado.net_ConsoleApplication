
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sql_Ado.net
{
    internal class Program
    {

        static void Main(string[] args)
        {

            Program.connection();
            //Console.ReadKey();
        }

        public static void connection()
        {
            string conn = ConfigurationManager.ConnectionStrings["Sql"].ConnectionString;
            //SqlConnection con = new SqlConnection(conn);
            SqlConnection con = null;
            try
            {
                using (con = new SqlConnection(conn))
                {
                    con.Open();
                    if(con.State == System.Data.ConnectionState.Open)
                    {
                        con.Close();
                        Console.WriteLine("Do you want to do\n 1.Insert Record \n 2.View Record \n 3.Update Record \n 4.Delete Record");
                        int Userinput = int.Parse(Console.ReadLine());
                        do
                        {
                            switch (Userinput)
                            {
                                case 1:
                                    Console.WriteLine("Enter the Employe name");
                                    string empName = Convert.ToString(Console.ReadLine());
                                    empName = char.ToUpper(empName[0]) + empName.Substring(1, empName.Length - 1);
                                    Console.WriteLine("Enter the Address");
                                    string empAddress = Convert.ToString(Console.ReadLine());

                                    SqlCommand cmd = new SqlCommand("insert into EmpLists values(@empName,@empAddress)", con);
                                    cmd.Parameters.AddWithValue("@empName", empName);
                                    cmd.Parameters.AddWithValue("@empAddress", empAddress);
                                    con.Open();
                                    int check = cmd.ExecuteNonQuery();
                                    if (check > 1)
                                    {
                                        Console.WriteLine("Inserted Successfully");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Insertion Failed");
                                    }

                                    break;

                                case 2:
                                    SqlCommand com = new SqlCommand("sp_employee", con);//stored procedure command
                                    con.Open();
                                    SqlDataReader dr = com.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        Console.WriteLine("Id= " + dr["Employee_id"] + "\n" + "Name= " + dr["Employee_Name"] + "\n" + "Address= " + dr["Address"]);
                                    }
                                    
                                    break;

                                case 3:
                                    Console.WriteLine("Enter the Employe Id");
                                    string empId = Console.ReadLine();
                                    SqlCommand findCmd = new SqlCommand("select * from EmpLists where Employee_id = @empId", con);
                                    findCmd.Parameters.AddWithValue("@empId", empId);
                                    con.Open();

                                    SqlDataReader reader = findCmd.ExecuteReader();

                                    if (reader.HasRows == true)
                                    {
                                        Console.WriteLine("Enter the Employe name");
                                        string updateEmpName = Convert.ToString(Console.ReadLine());
                                        updateEmpName = char.ToUpper(updateEmpName[0]) + updateEmpName.Substring(1, updateEmpName.Length - 1);
                                        Console.WriteLine("Enter the Address");
                                        string updateEmpAddress = Convert.ToString(Console.ReadLine());
                                        SqlCommand updateCmd = new SqlCommand("update EmpLists set Employee_Name=@updateEmpName,Address=@updateEmpAddress where Employee_id=@empId", con);
                                        updateCmd.Parameters.AddWithValue("@empId", empId);
                                        updateCmd.Parameters.AddWithValue("@updateEmpName", updateEmpName);
                                        updateCmd.Parameters.AddWithValue("@updateEmpAddress", updateEmpAddress);
                                        con.Close();
                                        con.Open();
                                        int chck = updateCmd.ExecuteNonQuery();
                                        if (chck > 0)
                                        {
                                            Console.WriteLine("Updated Successfully");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Something went wrong!!");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Data Not Found!!");
                                    }
                                    //SqlCommand updateCmd = new SqlCommand("update ", con);

                                    break;

                                case 4:
                                    Console.WriteLine("Enter the Employe Id");
                                    string DeleteEmpId = Console.ReadLine();
                                    SqlCommand findIdCmd = new SqlCommand("select * from EmpLists where Employee_id= @DeleteEmpId", con);
                                    findIdCmd.Parameters.AddWithValue("@DeleteEmpId", DeleteEmpId);
                                    con.Open();

                                    SqlDataReader reader2 = findIdCmd.ExecuteReader();

                                    if (reader2.HasRows == true)
                                    {
                                        SqlCommand DeleteCmd = new SqlCommand("delete from EmpLists where Employee_id = @DeleteEmpId", con);
                                        DeleteCmd.Parameters.AddWithValue("@DeleteEmpId", DeleteEmpId);
                                        con.Close();
                                        con.Open();
                                        int checkDeleted = DeleteCmd.ExecuteNonQuery();
                                        if (checkDeleted > 0)
                                        {
                                            Console.WriteLine("Deleted Successfully");
                                        }
                                        else
                                        {
                                            Console.WriteLine("Something went wrong");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Data Not Found!!");
                                    }
                                    break;
                                case 5:
                                    Environment.Exit(0);
                                    break;
                                default:
                                    Console.WriteLine("Invalid Input or Something went wrong!!!!!!!!");
                                    break;

                            }
                            con.Close();
                            Console.WriteLine("\nFor Exit type 5");
                            Userinput = int.Parse(Console.ReadLine());
                            
                            
                        } while(Userinput != 5);

                    }
                    
                }
            }
            catch(Exception ex)
            {
                var msg = ex.Message;
                Console.WriteLine(msg);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
