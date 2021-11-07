using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioCreador : Base, IRepositorioCreador
    {
        public RepositorioCreador(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Creador creador){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO creadores(nick_name, mail, 
                password, nombre, apellido, avatar)
                VALUES(@nick, @mail, @password, @nombre, @apellido, @avatar);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nick", creador.NickName);
                    comm.Parameters.AddWithValue("@mail", creador.Mail);
                    comm.Parameters.AddWithValue("@password", creador.Password);
                    comm.Parameters.AddWithValue("@nombre", creador.Nombre);
                    comm.Parameters.AddWithValue("@apellido", creador.Apellido);
                    if (String.IsNullOrEmpty(creador.Avatar)){
                        comm.Parameters.AddWithValue("@avatar", DBNull.Value);
                    }else{
                        comm.Parameters.AddWithValue("@avatar", creador.Avatar);
                    }
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    creador.Id=res;
                }
            }
            return res;
        }
        public int Baja(Creador creador){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM creadores WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", creador.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Creador creador){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE creadores
                SET nick_name=@nick, mail=@mail, password=@password, 
                nombre=@nombre, apellido=@apellido, avatar=@avatar
                WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nick", creador.NickName);
                    comm.Parameters.AddWithValue("@mail", creador.Mail);
                    comm.Parameters.AddWithValue("@password", creador.Password);
                    comm.Parameters.AddWithValue("@nombre", creador.Nombre);
                    comm.Parameters.AddWithValue("@apellido", creador.Apellido);
                    comm.Parameters.AddWithValue("@avatar", creador.Avatar);
                    comm.Parameters.AddWithValue("@id", creador.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Creador> ObtenerTodos(){
            IList<Creador> lista = new List<Creador>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail,
                    nombre, apellido, avatar 
                    FROM creadores";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        var creador = new Creador{
                            Id= reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Nombre = reader.GetString(3),
                            Apellido = reader.GetString(4),
                            Avatar = reader[nameof(Creador.Avatar)] == DBNull.Value ? null : reader.GetString(5)
                        };
                        lista.Add(creador);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Creador ObtenerPorId(int id){
            Creador creador = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail, password,
                    nombre, apellido, avatar 
                    FROM creadores
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        creador = new Creador{
                            Id= reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Password = reader.GetString(3),
                            Nombre = reader.GetString(4),
                            Apellido = reader.GetString(5),
                            Avatar = reader[nameof(Creador.Avatar)] == DBNull.Value ? null : reader.GetString(6)
                        };
                    }
                    conn.Close();
                }
            }
            return creador;
        }
        public Creador ObtenerPorMail(string mail){
            Creador creador = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail, password,
                    nombre, apellido, avatar 
                    FROM creadores
                    WHERE mail=@mail";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@mail", MySqlDbType.Int32).Value = mail;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        creador = new Creador{
                            Id= reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Password = reader.GetString(3),
                            Nombre = reader.GetString(4),
                            Apellido = reader.GetString(5),
                            Avatar = reader[nameof(Creador.Avatar)] == DBNull.Value ? null : reader.GetString(6)
                        };
                    }
                    conn.Close();
                }
            }
            return creador;
        }
    }
}