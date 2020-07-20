using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VendedorInterespacial
{
    public partial class VendedorInterespacial : Form
    {
        public VendedorInterespacial()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        Dictionary<String, Int32> _nmbrs = new Dictionary<string, int>()
        {
            {"I", 1 },{"V", 5 },{"X",10},{"L",50 },{"C",100 },{"D",500 },{"M",1000}
        };

        Dictionary<String, String> _represents = new Dictionary<string, string>();
        Dictionary<String, Double> _mineralValue = new Dictionary<string, double>(); //unit value of mineral
        String newLine = Environment.NewLine;

        private void btnExecutar_Click(object sender, EventArgs e)
        {
            this.textOutput.Text = ""; //cleans text Output
            _represents = new Dictionary<string, string>();
            _mineralValue = new Dictionary<string, double>(); //unit value of mineral

            var entry_lines = this.textInput.Text.Split('\n'); //splits lines in entries

            foreach (var entry in entry_lines)
            {
                var splitEntry = entry.Split(' ');
                if (splitEntry.Contains("representa"))
                {
                    InsertRepresentation(splitEntry[0], splitEntry[2]);
                }
                else if (splitEntry.Contains("valem"))
                {
                    int index = Array.IndexOf<String>(splitEntry, "valem");
                    String[] valuesnMineral = new string[index];
                    for (int i = 0; i < index; i++)
                    {
                        valuesnMineral[i] = splitEntry[i];
                    }
                    Int32 credits = Convert.ToInt32(splitEntry[index + 1]);

                    CalculateQuantity(valuesnMineral, credits);
                }
                else if (splitEntry.Contains("quanto"))
                {
                    //quanto vale .... ?
                    QuantoVale(splitEntry);
                }
                else
                { //quantos creditos sao
                    QuantosCreditosSao(splitEntry);
                }
            }

        }

        private void InsertRepresentation(string name, string equivalent)
        {
            if (_represents.Count < 7)
            {
                if (equivalent.Contains("\r") || equivalent.Contains("\n"))
                {
                    equivalent = equivalent.Substring(0, 1);
                }
                equivalent = equivalent.ToUpper();
                _represents.Add(name, equivalent);
            }
        }

        //calculates the value of a single mineral from the mineral type and saves it to the dictionary mineralValue
        private void CalculateQuantity(String[] valuesAndMineral, Int32 numberCredits)
        {
            String mineral = valuesAndMineral.Last();

            Int32 value = 0;
            //ignores the last element which is the mineral name
            for (int i=0; i<valuesAndMineral.Count()-1; i++)
            {                                    //number value of [ representation value of [ alien notation] ] 
                var integerNotation = _nmbrs[_represents[valuesAndMineral[i]]];
                if (i < valuesAndMineral.Count() - 2)
                {                                                   //next number
                    if (integerNotation < _nmbrs[_represents[valuesAndMineral[i + 1]]])
                    {
                        integerNotation = -integerNotation; //if its smaller than the next number, its a subtraction
                    }
                }
                value += integerNotation;
            }
            double finalValue = Convert.ToDouble(numberCredits) / Convert.ToDouble(value);
            try
            {
                _mineralValue.Add(mineral, finalValue);
            }
            catch (Exception ex)
            {
                this.textOutput.AppendText(ex.Message + newLine);
            }
        }

        private void QuantoVale(string[] qtVale)
        {
            try
            {
                Int32 value = 0;
                for (int i = 2; i < qtVale.Count() - 1; i++)
                {
                    //number value of [ representation value of [ alien notation] ] 
                    var integerNotation = _nmbrs[_represents[qtVale[i]]];
                    if (i < qtVale.Count() - 2)
                    {                                                   //next number
                        if (integerNotation < _nmbrs[_represents[qtVale[i + 1]]])
                        {
                            integerNotation = -integerNotation; //if its smaller than the next number, its a subtraction
                        }
                    }
                    value += integerNotation;
                }
                StringBuilder sb = new StringBuilder();
                for (int i =2; i<qtVale.Count()-1; i++)
                {
                    sb.Append(qtVale[i] + " ");
                }
                sb.Append("vale "+ Convert.ToString(value));

                this.textOutput.AppendText(sb.ToString() + newLine);
            }
            catch (Exception)
            {
                this.textOutput.AppendText("Nem ideia do que isto significa!"+newLine);
            }
        }

        private void QuantosCreditosSao(string[] qtSao)
        {
            try
            {
                //how many minerals
                Int32 qts = 0;
                for (int i = 3; i < qtSao.Count() - 2; i++)
                {
                    //number value of [ representation value of [ alien notation] ] 
                    var integerNotation = _nmbrs[_represents[qtSao[i]]];
                    if (i < qtSao.Count() - 3)
                    {                                                   //next number
                        if (integerNotation < _nmbrs[_represents[qtSao[i + 1]]])
                        {
                            integerNotation = -integerNotation; //if its smaller than the next number, its a subtraction
                        }
                    }
                    qts += integerNotation;
                }
                String mineral = qtSao[qtSao.Count()-2];
                Double value = _mineralValue[mineral] * qts;


                StringBuilder sb = new StringBuilder();
                for (int i = 3; i < qtSao.Count() - 1; i++)
                {
                    sb.Append(qtSao[i] + " ");
                }
                sb.Append("custa " + Convert.ToString(value) + " créditos");

                this.textOutput.AppendText(sb.ToString() + newLine);
            }
            catch (Exception)
            {
                this.textOutput.AppendText("Nem ideia do que isto significa!"+ newLine);
            }
        }

    }
}
