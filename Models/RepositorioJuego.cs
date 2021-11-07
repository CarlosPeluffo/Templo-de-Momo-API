using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace Templo_de_Momo.Models
{
    public class RepositorioJuego : Base, IRepositorioJuego
    {
        public RepositorioJuego(IConfiguration configuration) : base(configuration)
        {
            
        }
        public int Alta(Juego juego){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"INSERT INTO juegos(CreadorId, titulo, portada, 
                        descripcion, requisitos, precio)
                        VALUES(@creador, @titulo, @portada, @descripcion, @requisitos, @precio);
                        SELECT last_insert_id();";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@creador", juego.CreadorId);
                    comm.Parameters.AddWithValue("@titulo", juego.Titulo);
                    if (String.IsNullOrEmpty(juego.Portada)){
                        comm.Parameters.AddWithValue("@portada", DBNull.Value);
                    }else{
                        comm.Parameters.AddWithValue("@portada", juego.Portada);
                    }
                    comm.Parameters.AddWithValue("@descripcion", juego.Descripcion);
                    comm.Parameters.AddWithValue("@requisitos", juego.Requisitos);
                    comm.Parameters.AddWithValue("@precio", juego.Precio);
                    conn.Open();
                    res = Convert.ToInt32(comm.ExecuteScalar());
                    conn.Close();
                    juego.Id=res;
                }
            }
            return res;
        }
        public int Baja(Juego juego){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql = @"DELETE FROM juegos WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@id", juego.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public int Modificacion(Juego juego){
            int res = -1;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"UPDATE juegos
                    SET CreadorId=@creador, titulo=@titulo, portada=@portada,
                    descripcion=@descripcion, requisitos=@requisitos, precio=@precio
                    WHERE id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.AddWithValue("@creador", juego.CreadorId);
                    comm.Parameters.AddWithValue("@titulo", juego.Titulo);
                    comm.Parameters.AddWithValue("@portada", juego.Portada);
                    comm.Parameters.AddWithValue("@descripcion", juego.Descripcion);
                    comm.Parameters.AddWithValue("@requisitos", juego.Requisitos);
                    comm.Parameters.AddWithValue("@precio", juego.Precio);
                    comm.Parameters.AddWithValue("@id", juego.Id);
                    conn.Open();
                    res = comm.ExecuteNonQuery();
                    conn.Close();
                }
            }
            return res;
        }
        public IList<Juego> ObtenerTodos(){
            IList<Juego> lista = new List<Juego>();
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"SELECT j.id, CreadorId, titulo, portada, descripcion, 
                        requisitos, precio, c.nick_name
                        FROM juegos j
                        INNER JOIN creadores c ON j.CreadorId = c.id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    while(reader.Read()){
                        Juego juego = new Juego{
                            Id = reader.GetInt32(0),
                            CreadorId = reader.GetInt32(1),
                            Titulo = reader.GetString(2),
                            Portada = reader[nameof(Juego.Portada)] == DBNull.Value ? null : reader.GetString(3),
                            Descripcion = reader.GetString(4),
                            Requisitos = reader.GetString(5),
                            Precio = reader.GetDouble(6),
                            Creador = new Creador{
                                Id = reader.GetInt32(1),
                                NickName = reader.GetString(7)
                            } 
                        };
                        lista.Add(juego);
                    }
                    conn.Close();
                }
            }
            return lista;
        }
        public Juego ObtenerPorId(int id){
            Juego juego = null;
            using(MySqlConnection conn = new MySqlConnection(connectionString)){
                string sql =@"SELECT j.id, CreadorId, titulo, portada, descripcion, 
                        requisitos, precio, c.nick_name
                        FROM juegos j
                        INNER JOIN creadores c ON j.CreadorId = c.id
                        WHERE j.id=@id";
                using(MySqlCommand comm = new MySqlCommand(sql, conn)){
                    comm.CommandType = CommandType.Text;
                    comm.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
                    conn.Open();
                    var reader = comm.ExecuteReader();
                    if(reader.Read()){
                        juego = new Juego{
                            Id = reader.GetInt32(0),
                            CreadorId = reader.GetInt32(1),
                            Titulo = reader.GetString(2),
                            Portada = reader[nameof(Juego.Portada)] == DBNull.Value ? null : reader.GetString(3),
                            Descripcion = reader.GetString(4),
                            Requisitos = reader.GetString(5),
                            Precio = reader.GetDouble(6),
                            Creador = new Creador{
                                Id = reader.GetInt32(1),
                                NickName = reader.GetString(7)
                            } 
                        };
                    }
                    conn.Close();
                }
            }
            return juego;
        }
    }
}