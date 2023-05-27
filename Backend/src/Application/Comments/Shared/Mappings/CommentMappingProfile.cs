namespace Application.Comments.Shared.Mappings;

public class CommentMappingProfile : Profile
{
	public CommentMappingProfile()
	{
		CreateMap<Comment, CommentViewModel>().ReverseMap();
	}
}
