namespace Curriculo4Dev.Core.Application.DataTransferObjects.Templates
{
    public class TemplateGetDto : BaseDto
    {
        public TemplateGetDtoAtributos Atributos { get; set; } = new();
    }

    public class TemplateGetDtoAtributos
    {
        public string Descricao { get; set; } = string.Empty;

        public string UrlImagemS3 { get; set; } = string.Empty;
    }
}
