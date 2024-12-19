using DTO;
using Transportes_API.Models;

namespace Transportes_API.Services
{
    public interface ICamiones
    {
        //es una estructura que define un contrato o conjunto de métodos y
        //propiedades que una clase debe implementar.
        //Una interfaz establece un conjunto de requisitos que cualquier clase
        //que la implemente debe seguir. Estos requisitos son declarados en la
        //interfaz en forma de firmas de métodos y propiedades,
        //pero la interfaz en sí misma no proporciona ninguna implementación
        //de estos métodos o propiedades.Es responsabilidad de las clases que
        //implementan la interfaz proporcionar las implementaciones concretas de
        //estos miembros.

        //Las interfaces son útiles para lograr la abstracción y la reutilización
        //de código en C#.

        //GET

        List<Camiones_DTO> GetCamiones();



        //GETbyID

        Camiones_DTO GetCamion(int id);

       
        //INSERT (POST)

        string InsertCamion(Camiones_DTO camion);



        //UPDATE (PUT)


        string UpdateCamion(Camiones_DTO camion);


        //DELETE(DELETE)

        string DeleteCamion(int id);


    }


    //La clase que implementa la interfaz y declara la implementacion de la logica de lod métodos exstentes

    public class CamionesService : ICamiones
    {
        //Variable par crear el contexto (Inyeccion de dependencias)
        private readonly TransportesContext _context;


        //constructor para inicializar el contexto
        public CamionesService(TransportesContext context)
        {
            _context = context;
        }
        //implementacion de métodos

        public string DeleteCamion(int id)
        {
            try
            {
                //Obtengo primero el camion de la basa de datos
                Camiones _camion = _context.Camiones.Find(id);
                //Valido que realmente recuperé mi obeto
                if (_camion == null)
                {
                    return $"No se encontró algún objeto con identificador{id}";
                }

                //Remuevo el objeto del contexto
                _context.Camiones.Remove(_camion);
                //Impacto la BD
                _context.SaveChanges();
                //Respondo
                return $"Camión {id} eliminado con éxito";


            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }

        }


        public Camiones_DTO GetCamion(int id)
        {
            Camiones origen = _context.Camiones.Find(id);
            //Dynamic Mapper
            Camiones_DTO resultado = DynamicMapper.Map<Camiones, Camiones_DTO>(origen);
            return resultado;

        }

        public List<Camiones_DTO> GetCamiones()
        {
            try
            {
                //Lista de camiones del original
                List<Camiones> lista_original = _context.Camiones.ToList();
                //Lista de DTOS
                List<Camiones_DTO> lista_salida = new List<Camiones_DTO>();
                //Recorro cada camión y genero un nuevo DTO con DynamicMapper
                foreach (var cam in lista_original)
                {
                    //usamos el dynamicmapper para convertir los objetos
                    Camiones_DTO DTO = DynamicMapper.Map<Camiones, Camiones_DTO>(cam);
                    lista_salida.Add(DTO);


                }

                //Retorno la lista con los objetos ya mapeados
                return lista_salida;

            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }

        public string InsertCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camión del modelo original
                Camiones _camion = new Camiones();
                //Asigno los valores del objeto DTO del parámetro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                //añadimos el objeto al contexto
                _context.Camiones.Add(_camion);
                //impactamos la BD
                _context.SaveChanges();
                //respondo
                return "Camión insertado con éxito";

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }

        public string UpdateCamion(Camiones_DTO camion)
        {
            try
            {
                //creo un camión del modelo original
                Camiones _camion = new Camiones();
                //Asigno los valores del objeto DTO del parámetro al objeto del modelo original
                _camion = DynamicMapper.Map<Camiones_DTO, Camiones>(camion);
                //Modifico el estado del objeto en el contexto
                _context.Entry(_camion).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                //impactamos la BD
                _context.SaveChanges();
                //respondo
                return $"Camión {_camion.ID_Camion} Actualizado con éxito";

            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}
