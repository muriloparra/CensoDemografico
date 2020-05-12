using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CensoDemografico.Domain.Enums
{
    public enum EEscolaridade
    {
        [Description("Sem Instrução")]
        SemInstrucao = 1,
        [Description("Ensino Fundamental")]
        EnsinoFundamental,
        [Description("Ensino Médio")]
        EnsinoMedio,
        [Description("Superior Completo")]
        SuperiorCompleto,
        [Description("Pós-Graduado")]
        PosGraduado,
        [Description("Doutorado")]
        Doutorado
    }
}
