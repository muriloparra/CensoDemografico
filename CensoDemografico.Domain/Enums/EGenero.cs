using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CensoDemografico.Domain.Enums
{
    public enum EGenero
    {
        [Description("Feminino")]
        Feminino = 1,
        [Description("Masculino")]
        Masculino,
        [Description("Não Informado")]
        NaoInformado
    }
}
