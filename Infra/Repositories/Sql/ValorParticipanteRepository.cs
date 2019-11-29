using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Sql;
using Core.Interfaces.Repositories.Dapper;
using Core.Interfaces.Repositories.Sql;
using Core.ViewModels.Evento;
using Infra.Contexts;
using Infra.Repositories.Dapper;
using Microsoft.Extensions.Configuration;

namespace Infra.Repositories.Sql
{
    public class ValorParticipanteRepository : Repository<ValorParticipante>, IValorParticipanteRepository
    {
        private readonly ISqlHelper _sqlHelper;

        public ValorParticipanteRepository(ITrincaContext context, IConfiguration configuration) : base(context)
        {
            _sqlHelper = new SqlHelper(configuration, "Trinca");
        }

        public async Task<List<ParticipanteEventoResponse>> BuscarParticipantes(int idEvento)
        {
            var query = $@"
                        SELECT 
		                    p.nome_completo AS NomeCompleto,
		                    p.apelido AS Apelido,
		                    p.email AS Email,
		                    v.valor AS Valor,
		                    v.valor_sugerido AS ValorSugerido
	                    FROM 
		                    valor_participante v
	                    JOIN 
		                    participante p ON p.id = v.id_participante
	                    WHERE v.id_evento = {idEvento}";

            var retorno = _sqlHelper.Query<ParticipanteEventoResponse>(query).ToList();

            return retorno;
        }

        public async Task RemoveParticipante(RemovePartcipanteEventoRequest request)
        {
            var query = $@"
                        DELETE 
                            valor_participante 
                        WHERE 
                            id_evento = {request.IdEvento} 
                            AND 
                            id_participante = {request.IdParticipante}";

            _sqlHelper.QuerySingle<int>(query);

            return;
        }
    }
}