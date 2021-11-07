using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioUsuario : Base, IRepositorioUsuario
    {
        public RepositorioUsuario(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"INSERT INTO usuarios(nick_name, mail, password, avatar)
                VALUES(@nick, @mail, @password, @avatar);
                SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nick", user.NickName);
                    comm.Parameters.AddWithValue("@mail", user.Mail);
                    comm.Parameters.AddWithValue("@password", user.Password);
                    if (String.IsNullOrEmpty(user.Avatar)){
                        comm.Parameters.AddWithValue("@avatar", DBNull.Value);
                    }else{
                        comm.Parameters.AddWithValue("@avatar", user.Avatar);
                    }
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    user.Id = res;
                }
            }
            return res;
        }
        public int Baja(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM usuarios WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", user.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Usuario user){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"UPDATE usuarios
                SET nick_name=@nick, mail=@mail, password=@password, avatar=@avatar
                WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@nick", user.NickName);
                    comm.Parameters.AddWithValue("@mail", user.Mail);
                    comm.Parameters.AddWithValue("@password", user.Password);
                    comm.Parameters.AddWithValue("@avatar", user.Avatar);
                    comm.Parameters.AddWithValue("@id", user.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Usuario> ObtenerTodos(){
            IList<Usuario> lista = new List<Usuario>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail, avatar
                FROM usuarios";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        var user = new Usuario{
                            Id = reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Avatar = reader[nameof(Usuario.Avatar)] == DBNull.Value ? null : reader.GetString(3),
                        };
                        lista.Add(user);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Usuario ObtenerPorId(int id){
            Usuario user = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail, password, avatar 
                    FROM usuarios
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        user = new Usuario{
                            Id= reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Password = reader.GetString(3),
                            Avatar = reader[nameof(Creador.Avatar)] == DBNull.Value ? null : reader.GetString(4)
                        };
                    }
                    conn.Close();
                }
            }
            return user;
        }
        public Usuario ObtenerPorMail(string mail){
            Usuario user = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"SELECT id, nick_name, mail, password, avatar 
                    FROM usuarios
                    WHERE mail=@mail";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@mail", MySqlDbType.Int32).Value = mail;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        user = new Usuario{
                            Id= reader.GetInt32(0),
                            NickName = reader.GetString(1),
                            Mail = reader.GetString(2),
                            Password = reader.GetString(3),
                            Avatar = reader[nameof(Creador.Avatar)] == DBNull.Value ? null : reader.GetString(4)
                        };
                    }
                    conn.Close();
                }
            }
            return user;
        }
    }
}