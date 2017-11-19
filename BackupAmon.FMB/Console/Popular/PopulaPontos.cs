using System;
using Amon.PontoE.Modelo.Cadastro;
using Amon.PontoE.Modelo.Ponto;
using Amon.PontoE.Persistencia.Cadastro;
using Amon.PontoE.Persistencia.Ponto;

namespace Console.Popular
{
    public class PopulaPontos
    {

        private DateTime data = new DateTime(2014, 01, 01);
        private readonly DateTime dataFinal = new DateTime(2014, 12, 31);
        private Funcionario Funcionario { get; set; }
        private JornadaTrabalho jornada { get; set; }

        static readonly Random rnd = new Random();

        private readonly JornadaTrabalhoDAO jornadaDAO;
        private readonly BatidaDAO batidaDAO;

        public PopulaPontos()
        {
            this.jornadaDAO = new JornadaTrabalhoDAO();
            this.batidaDAO = new BatidaDAO();
        }

        public void popular(String idFuncionario)
        {
            this.Funcionario = new Funcionario { Id = idFuncionario };
            this.Funcionario = new FuncionarioDAO().obter(Funcionario);
            this.jornada = jornadaDAO.obter(new JornadaTrabalho { Id = Funcionario.IdJornada });

            System.Console.WriteLine("Batendo pontos para o funcionário {0}...", Funcionario.Nome);

            TimeSpan tolerancia = new TimeSpan(0, 5, 0);

            while (data <= dataFinal)
            {
                if (data.DayOfWeek != DayOfWeek.Saturday || data.DayOfWeek != DayOfWeek.Sunday)
                {
                    Batida entrada = new Batida
                    {
                        Data = data,
                        Hora = geraRandom15Min(jornada.Inicio, 5),
                        IdFuncionario = Funcionario.Id
                    };

                    Batida saida = new Batida
                    {
                        Data = data,
                        Hora = geraRandom15Min(jornada.Fim, 5),
                        IdFuncionario = Funcionario.Id
                    };

                    Batida saidaIntervalo = new Batida
                    {
                        Data = data,
                        Hora = geraRandomSaidaIntervalo(jornada.MinIntervalo, tolerancia),
                        IdFuncionario = Funcionario.Id
                    };

                    Batida entradaIntervalo = new Batida
                    {
                        Data = data,
                        Hora = geraRandomEntradaIntervalo(saidaIntervalo.Hora, jornada.Intervalo),
                        IdFuncionario = Funcionario.Id
                    };

                    batidaDAO.incluir(entrada);
                    batidaDAO.incluir(saida);
                    batidaDAO.incluir(entradaIntervalo);
                    batidaDAO.incluir(saidaIntervalo);
                    System.Console.WriteLine("Processando dia {0:dd/MM/yyyy}... Entrada: {1:hh\\:mm} - Entrada: {2:hh\\:mm} - Entrada: {3:hh\\:mm} - Entrada: {4:hh\\:mm}",
                        data, entrada.Hora, saidaIntervalo.Hora, entradaIntervalo.Hora, saida.Hora);
                }
                else
                {
                    System.Console.WriteLine("Processando dia {0}, Final de Semana.");
                }

                data = data.AddDays(1);
            }

        }

        public TimeSpan geraRandom15Min(TimeSpan hora, int variacao)
        {
            return hora.Add((new TimeSpan(0, rnd.Next(-variacao, variacao), 0)));
        }

        public TimeSpan geraRandomSaidaIntervalo(TimeSpan min, TimeSpan max)
        {
            return new TimeSpan(rnd.Next(min.Hours, (min.Hours + max.Hours)), rnd.Next(0, 59), 0);
        }

        public TimeSpan geraRandomEntradaIntervalo(TimeSpan saidaInt, int intevalo)
        {
            return saidaInt.Add((new TimeSpan(0, intevalo ,0)));
        }

    }
}
