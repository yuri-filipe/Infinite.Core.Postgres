﻿namespace Infinite.Core.Postgres.Mapping
{
    public abstract class CoreEntity
    {
        public abstract long Id { get; set; }
        public bool Excluido { get; set; }
        public string UsuarioInclusao { get; set; }
        public DateTime DataInclusao { get; set; }
        public string UsuarioAlteracao { get; set; }
        public DateTime DataAlteracao { get; set; }
    }
}