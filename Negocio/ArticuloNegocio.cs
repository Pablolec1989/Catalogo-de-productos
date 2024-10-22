using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using dominio;

namespace negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, IdCategoria, C.Descripcion Categoria, IdMarca, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = M.Id");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    //VALIDAR NULOS
                    if (!(datos.Lector["Codigo"] is DBNull))
                        aux.Codigo = (string)datos.Lector["Codigo"];
                    
                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];

                    if (!(datos.Lector["Descripcion"] is DBNull))
                        aux.Descripcion = (string)datos.Lector["Descripcion"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    if (!(datos.Lector["Precio"] is DBNull))
                        aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);
                        
                        


                    if (!(datos.Lector["Categoria"] is DBNull))
                        aux.Category = new Categoria();
                        aux.Category.Id = (int)datos.Lector["IdCategoria"];
                        aux.Category.Descripcion = (string)datos.Lector["Categoria"];

                    if (!(datos.Lector["Categoria"] is DBNull))
                        aux.Mark = new Marca();
                        aux.Mark.Id = (int)datos.Lector["IdMarca"];
                        aux.Mark.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }
        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo, Nombre, Descripcion, IdMarca, IdCategoria, ImagenUrl, Precio) values (@codigo, @nombre, @descripcion, @idMarca, @idCategoria, @img, @precio)");
                datos.setearParametro("@codigo", nuevo.Codigo);
                datos.setearParametro("@nombre", nuevo.Nombre);
                datos.setearParametro("@descripcion", nuevo.Descripcion);
                datos.setearParametro("@idMarca", nuevo.Mark.Id);
                datos.setearParametro("@idCategoria", nuevo.Category.Id);
                datos.setearParametro("@img", nuevo.ImagenUrl);
                datos.setearParametro("@precio", nuevo.Precio);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Modificar(Articulo articulo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("update ARTICULOS set Codigo = @codigo, Nombre= @nombre, Descripcion = @descripcion , IdMarca= @idMarca, IdCategoria= @idCategoria, ImagenUrl=@img, Precio= @precio where Id = @id");
                datos.setearParametro("@codigo", articulo.Codigo);
                datos.setearParametro("@nombre", articulo.Nombre);
                datos.setearParametro("@descripcion", articulo.Descripcion);
                datos.setearParametro("@idMarca", articulo.Mark.Id);
                datos.setearParametro("@idCategoria", articulo.Category.Id);
                datos.setearParametro("@img", articulo.ImagenUrl);
                datos.setearParametro("@precio", articulo.Precio);
                datos.setearParametro("@id", articulo.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete From ARTICULOS where id=@id");
                datos.setearParametro("@id", id);
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtro)
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select A.Id, Codigo, Nombre, A.Descripcion, ImagenUrl, Precio, IdCategoria, C.Descripcion Categoria, IdMarca, M.Descripcion Marca From ARTICULOS A, CATEGORIAS C, MARCAS M where A.IdCategoria = C.Id and A.IdMarca = M.Id and ";

                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "$ 0 - 100.000":
                            consulta += "Precio BETWEEN 0 AND 100.000 " + filtro;
                            break;

                        case "$ 301 - 500":
                            consulta += "Precio BETWEEN 101.000 AND 200.000 " + filtro;
                            break;

                        default:
                            consulta += "Precio >201.000 " + filtro;
                            break;
                    }
                }else if(campo == "Codigo")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Codigo like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Codigo like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "Codigo like '%" + filtro + "%'";
                            break;
                    }
                }
                else if (campo == "Categoria")
                {
                    switch (criterio)
                    {
                        case "Celulares":
                            consulta += "C.Descripcion = " + "'" + criterio + "'";
                            break;
                        case "Televisores":
                            consulta += "C.Descripcion = " + "'" + criterio + "'";
                            break;
                        case "Media":
                            consulta += "C.Descripcion = " + "'" + criterio + "'";
                            break;
                        default:
                            consulta += "C.Descripcion = " + "'" + criterio + "'";
                            break;
                    }
                }
                else if (campo == "Marca")
                {
                    switch (criterio)
                    {
                        case "Samsung":
                            consulta += "M.Descripcion = " + "'" + criterio + "'";
                            break;
                        case "Apple":
                            consulta += "M.Descripcion = " + "'" + criterio + "'";
                            break;
                        case "Sony":
                            consulta += "M.Descripcion = " + "'" + criterio + "'";
                            break;
                        case "Huawei":
                            consulta += "M.Descripcion = " + "'" + criterio + "'";
                            break;
                        default:
                            consulta += "M.Descripcion = " + "'" + criterio + "'";
                            break;
                    }
                }
                else if(campo == "Nombre")
                {
                    switch (criterio)
                    {
                        case "Comienza con":
                            consulta += "Nombre like '" + filtro + "%'";
                            break;
                        case "Termina con":
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                        default:
                            consulta += "Nombre like '%" + filtro + "%'";
                            break;
                    }
                }
                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Codigo = (string)datos.Lector["Codigo"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];
                    aux.Precio = Math.Round((decimal)datos.Lector["Precio"], 2);

                    aux.Category = new Categoria();
                    aux.Category.Id = (int)datos.Lector["IdCategoria"];
                    aux.Category.Descripcion = (string)datos.Lector["Categoria"];
                    aux.Mark = new Marca();
                    aux.Mark.Id = (int)datos.Lector["IdMarca"];
                    aux.Mark.Descripcion = (string)datos.Lector["Marca"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
