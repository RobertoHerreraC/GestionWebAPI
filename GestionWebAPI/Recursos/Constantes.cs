namespace GestionWebAPI.Recursos
{
    public class Constantes
    {
        public class Responsable
        {
            public const string FRAI = "FRAI";
            public const string MPV = "MPV";
            public const string RCL = "RCL";
            public const string DERIVADO = "DERIVADO";


        }

        public class CorreoCriterio
        {
            public const string REGISTRO = "REGISTRO";
            public const string VALIDACION_APROBADA = "VALIDACION_APROBADA";
            public const string VALIDACION_RECHAZADA = "VALIDACION_RECHAZADA";
            public const string REGISTRO_SIGEDD = "REGISTRO_SIGEDD";
            public const string DERIVAR_CLASIFICACION = "DERIVAR_CLASIFICACION";
            public const string CLASIFICACION_NO_PUB = "CLASIFICACION_NO_PUB";
            public const string PETICION_INFORMACION = "PETICION_INFORMACION";
            public const string COSTO_SOLICITUD = "COSTO_SOLICITUD";
            public const string ENVIO_INFORMACION = "ENVIO_INFORMACION";

        }

        public class CorreoEntidad
        {
            public const string ADMINISTRADO = "ADMINISTRADO";
            public const string FRAI = "FRAI";
            public const string MPV = "MPV";
            public const string RCL = "RCL";
            public const string DERIVADOS = "DERIVADOS";

        }

        public class Documento
        {
            public const string REGISTRO = "REGISTRO";
            public const string VALIDACION = "VALIDACION";
            public const string SIGEDD = "SIGEDD";
            public const string CLASIFICACION = "CLASIFICACION";
            public const string DERIVACION_CONSULTA = "DERIVACION_CONSULTA";
            public const string DERIVACION_RESPUESTA = "DERIVACION_RESPUESTA";
            public const string ACOPIO = "ACOPIO";
            public const string ACOPIO_TOTAL = "ACOPIO_TOTAL";
            public const string COSTO_SOLICITUD = "COSTO_SOLICITUD";
            public const string REGISTRO_VOUCHER = "REGISTRO_VOUCHER";
            public const string CONFIRMACION_ENTREGA = "CONFIRMACION_ENTREGA";

        }


        public class Respuestas
        {
            public const int VALIDACION_POSITIVA = 3;
            public const int VALIDACION_NEGATIVA = 2;
            public const int REGISTRO_SIGEDD = 5;
            public const int CLASIFICACION_DERIVACION = 6;
            public const int CLASIFICACION_POSITIVA = 12;
            public const int CLASIFICACION_NEGATIVA = 9;
            public const int DERIVAR_CONSULTA = 13;
            public const int DERIVAR_RESPUESTA = 14;
            public const int ACOPIO_INDIVIDUAL = 15;
            public const int ACOPIO_FINAL = 16;
            public const int REGISTRO_COSTO = 17;
            public const int REGISTRO_VOUCHER = 19;
            public const int ENTREGA_INFORMACION = 20;
            public const int RECEPCION_ENTREGA = 21;

        }
    }
}
