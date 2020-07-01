using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace teste
{
    class Plantao
    {

        public DateTime sabado { get; set; }
        public DateTime domingo { get; set; }
        public string plantonista { get; set; }

        public Plantao(DateTime Sabado, DateTime Domingo, String Plantonista)
        {
            this.sabado = Sabado;
            this.domingo = Domingo;
            this.plantonista = Plantonista;
        }

    }
    class PlantaoFeriado
    {
        public PlantaoFeriado() { }
        public DateTime feriado {get; set; }
        public String plantonista { get; set; }

        public PlantaoFeriado(DateTime Feriado, String Plantonista)
        {
            this.feriado = Feriado;
            this.plantonista = Plantonista;
        }
    }
}
