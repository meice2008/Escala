using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using System.IO;

namespace teste
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //altera a cultura para evitar data incorreta
            Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
            //exibe o ano atual no textbox
            ano.Text = DateTime.Now.Year.ToString();
        }

        DateTime varSab, varDom;
        List<Plantao> listaPlantao = new List<Plantao>();
        List<PlantaoFeriado> listaPlantaoFeriado = new List<PlantaoFeriado>();
        List<String> listaNomes = new List<String>();
        List<DateTime> listaFeriados = new List<DateTime>();

        public void carregarFeriados(int a)
        {
            listaFeriados.Add(new DateTime(a, 01, 01));//ano novo

            int mes, dia;
            int x = 24, y = 5;
            int aux = a % 19;
            int b = a % 4;
            int c = a % 7;
            int d = (19 * aux + x) % 30;
            int e = (2 * b + 4 * c + 6 * d + y) % 7;

            if ((d + e) > 9)
            {
                dia = (d + e - 9);
                mes = 4;
            }
            else
            {
                dia = (d + e + 22);
                mes = 3;
            }
            DateTime pascoa = new DateTime(a, mes, dia);
            DateTime carnaval = pascoa.AddDays(-47);
            DateTime paixao = pascoa.AddDays(-2);
            //DateTime corpus = pascoa.AddDays(60);

            listaFeriados.Add(carnaval);
            listaFeriados.Add(paixao);
            listaFeriados.Add(pascoa);
            listaFeriados.Add(new DateTime(a, 04, 21));//tiradentes
            listaFeriados.Add(new DateTime(a, 05, 01));//dia do trabalho            
            listaFeriados.Add(new DateTime(a, 09, 07));//independencia            
            listaFeriados.Add(new DateTime(a, 09, 08));//niver cwb
            listaFeriados.Add(new DateTime(a, 10, 12));//aparecida
            listaFeriados.Add(new DateTime(a, 11, 02));//finados
            listaFeriados.Add(new DateTime(a, 11, 15));//republica
            listaFeriados.Add(new DateTime(a, 12, 25));//natal            
        }
        //btnListar
        private void button1_Click(object sender, EventArgs e)
        {
            if (listaNomes.Count() == 0)
            {
                MessageBox.Show("Preencha o nome dos plantonistas antes de listar os plantoes");
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                int a = Convert.ToInt32(ano.Text);
                carregarFeriados(a);
                try
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo("pt-BR");
                    DateTime dtFim = new DateTime(a, 12, 31);
                    int i = 0;

                    for (DateTime dt = new DateTime(a, 1, 1); dt <= dtFim; dt = dt.AddDays(1))
                    {

                        if (dt.DayOfWeek == DayOfWeek.Saturday)
                        {
                            varSab = dt;
                        }
                        else if (dt.DayOfWeek == DayOfWeek.Sunday)
                        {
                            varDom = dt;
                        }
                        if ((varSab != DateTime.MinValue) && (varDom != DateTime.MinValue))
                        {
                            listaPlantao.Add(new Plantao(varSab, varDom, listaNomes[i]));
                            varSab = DateTime.MinValue;
                            varDom = DateTime.MinValue;
                            
                            i += 1;
                            while (i > listaNomes.Count() - 1)
                            {
                                i = 0;
                            }
                        }
                    }
                    dataGridView1.DataSource = listaPlantao;

                    for (int j = 0; j < listaFeriados.Count; j++)
                    {

                        if (listaFeriados[j].DayOfWeek == DayOfWeek.Monday || listaFeriados[j].DayOfWeek == DayOfWeek.Tuesday)
                        {
                            for (int k = 0; k < listaPlantao.Count; k++)
                            {
                                DateTime auxSab = listaPlantao[k].sabado;
                                DateTime auxDom = listaPlantao[k].domingo;
                                if (auxSab.AddDays(2) == listaFeriados[j] || auxSab.AddDays(3) == listaFeriados[j])
                                {
                                    listaPlantaoFeriado.Add(new PlantaoFeriado(listaFeriados[j], listaPlantao[k].plantonista));
                                }
                            }
                        }
                        else if (listaFeriados[j].DayOfWeek == DayOfWeek.Wednesday || listaFeriados[j].DayOfWeek == DayOfWeek.Thursday || listaFeriados[j].DayOfWeek == DayOfWeek.Friday)
                        {
                            for (int k = 0; k < listaPlantao.Count; k++)
                            {
                                DateTime auxSab = listaPlantao[k].sabado;
                                DateTime auxDom = listaPlantao[k].domingo;
                                if (auxSab.AddDays(-1) == listaFeriados[j] || auxSab.AddDays(-2) == listaFeriados[j] || auxSab.AddDays(-3) == listaFeriados[j])
                                {
                                    listaPlantaoFeriado.Add(new PlantaoFeriado(listaFeriados[j], listaPlantao[k].plantonista));
                                }
                            }
                        }
                        else if (listaFeriados[j].DayOfWeek == DayOfWeek.Saturday || listaFeriados[j].DayOfWeek == DayOfWeek.Sunday)
                        {
                            for (int k = 0; k < listaPlantao.Count; k++)
                            {
                                if (listaPlantao[k].sabado == listaFeriados[j])
                                {
                                    listaPlantaoFeriado.Add(new PlantaoFeriado(listaFeriados[j], listaPlantao[k].plantonista));
                                    MessageBox.Show("Feriado no sabado: " + listaFeriados[j] + "\n" + " Plantonista já escalado.");
                                }
                                else if (listaPlantao[k].domingo == listaFeriados[j])
                                {
                                    listaPlantaoFeriado.Add(new PlantaoFeriado(listaFeriados[j], listaPlantao[k].plantonista));
                                    MessageBox.Show("Feriado no domingo: " + listaFeriados[j] + "\n" + " Plantonista já escalado.");
                                }
                            }
                        }
                    }
                    dataGridView2.DataSource = listaPlantaoFeriado;

                }
                catch (FormatException)
                {
                    MessageBox.Show("O ano esta incorreto");
                    ano.Focus();
                }
            }
        }
        //btnAdicionar
        private void button1_Click_1(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("O nome não foi preenchido");
            }
            else if (!listBoxNomes.Items.Contains(txtNome.Text))
            {
                listaNomes.Add(txtNome.Text);
                listBoxNomes.Items.Add(txtNome.Text);
            }
            else
            {
                MessageBox.Show("Plantonista ja cadastrado");
            }
            txtNome.Text = String.Empty;
            txtNome.Focus();
        }
        //btnExportar
        private void button1_Click_2(object sender, EventArgs e)
        {
            TextWriter w = new StreamWriter(@"C:\Users\Usuario\Documents\Escala Plantao.txt");
            //grid fim de semana
            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                w.Write(dataGridView1.Columns[i].HeaderText + ";");                
            }
            w.WriteLine("\n");
            for (int i=0; i < dataGridView1.Rows.Count -1; i++)
            {                
                for (int j=0; j<dataGridView1.Columns.Count; j++)
                {
                    String aux;
                    if (dataGridView1.Rows[i].Cells[j].Value.GetType() == typeof(DateTime))
                    {
                        aux = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        DateTime q = DateTime.Parse(aux);
                        w.Write(q.ToShortDateString()+";");
                    }
                    else
                    {
                        aux = dataGridView1.Rows[i].Cells[j].Value.ToString();
                        w.Write(aux + ";");
                    }                    
                }
                w.WriteLine(";");
            }
            w.WriteLine("\n");

            //grid feriados
            for (int i = 0; i < dataGridView2.Columns.Count; i++)
            {
                w.Write(dataGridView2.Columns[i].HeaderText + ";");
            }
            w.WriteLine("\n");
            for (int i = 0; i < dataGridView2.Rows.Count - 1; i++)
            {
                for (int j = 0; j < dataGridView2.Columns.Count; j++)
                {
                    String aux;
                    if (dataGridView2.Rows[i].Cells[j].Value.GetType() == typeof(DateTime))
                    {
                        aux = dataGridView2.Rows[i].Cells[j].Value.ToString();
                        DateTime q = DateTime.Parse(aux);
                        w.Write(q.ToShortDateString() + ";");
                    }
                    else
                    {
                        aux = dataGridView2.Rows[i].Cells[j].Value.ToString();
                        w.Write(aux + ";");
                    }
                }
                w.WriteLine(";");
            }
            w.Close();
            MessageBox.Show("Exportado com sucesso! Verifique a pasta documentos");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
