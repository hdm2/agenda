using System;

namespace WFApp
{
    public class TextoNotificacao
    {
        public static String preJornada = "Você não pode usar a máquina antes do seu horário.\nSua entrada está marcada para as {0:hh\\:mm}h.\nVocê será desconectado do sistema agora.",
                             fimJornada = "Você chegou ao fim da sua jornada e bateu seu último ponto às {0:hh\\:mm}h.\nVocê será desconectado do sistema agora.",
                             proximidadeFim = "O fim da sua jornada é às {0:hh\\:mm}h.\nFaltam apenas {1} para seu horário de saída.\nSe desejar, pode clicar e bater o ponto agora.",
                             naToleranciaFim = "Você já está na tolerância e será desconectado do sistema às {0:hh\\:mm}h.\nVocê tem {1} para salvar seu trabalho e sair.\nSe desejar, pode clicar e bater o ponto.",
                             tempoRestanteFim = "Você tem apenas {0} antes de ser bloqueado.",
                             aposFim = "Você não pode utilizar a máquina depois do seu horário, sua jornada normal encerrou às {0:hh\\:mm}h.\nVocê será desconectado do sistema agora.",
                             inicio = "Você ainda não bateu ponto.\nClique neste aviso ou no ícone com o botão direito para bater o ponto agora.",
                             aposIntervaloMinimo = "Você já pode tirar seu intervalo a partir de agora até as {0:hh\\:mm}h.\nClique nesta notificação ou no ícone a qualquer momento para registrar sua saída para o intervalo.",
                             proximidadeIntervalo = "Você poderá sair para o intervalo dentro de {0}.",
                             aposIntervaloMaximo = "Você atingiu o horário máximo para o intervalo.\nBata o seu ponto clicando com o botão direito no ícone e retorne após {0} minutos de intervalo. Você será desconectado dentro de alguns segundos.",
                             duranteIntervalo = "Você não pode usar a máquina no seu horário de intervalo.\nSeu horário de retorno é após as {0:hh\\:mm}h.",
                             aposIntervalo = "Você ainda não bateu seu retorno do intervalo.\nSeu horário de retorno foi às {0:hh\\:mm}h. Clique neste aviso ou no ícone para bater o ponto agora.",
                             retornoIntervaloNaTolerancia = "Você ainda tem {0:mm\\:ss} minutos de intervalo, mas, se desejar, pode bater seu ponto agora.",
                             notificaAutorizacao = "Você possui uma autorização para utilizar a máquina das {0:hh\\:mm}h às {1:hh\\:mm}h.\nApós este período, a máquina será bloqueada.\nClique no ícone para bater o ponto.",
                             tempoRestanteAutorizacao = "Faltam apenas {0} para terminar seu período de desbloqueio.",
                             notificacaoHoraExtra = "Você poderá autorizar seus funcionários a fazerem horas extras, porém o desbloqueio deverá ser feito dentro do período permitido.\nClique neste aviso para ignorar esta mensagem.",
                             notificacaoNovoDesbloqueio = "Você possui uma autorização para utilizar a máquina no período das {0:hh\\:mm}h às {1:hh\\:mm}h.",
                             aguardoHoraExtra = "Você deve aguardar no mínimo {0} minutos para iniciar sua hora extra.",

                             inicioIntervaloProgramado = "Você já pode tirar seu intervalo programado de 10 minutos agora, clique no ícone para registrar o ponto.",
                             retornoIntervaloProgramado = "Você já pode registrar seu retorno do intervalo programado de 10 minutos agora, clique no ícone para registrar o ponto.",
                             temJornadaAlternativa = "Foi identificado que você está dentro de uma jornada alternativa. Você pode usar a máquina normalmente.";
    }
}
 