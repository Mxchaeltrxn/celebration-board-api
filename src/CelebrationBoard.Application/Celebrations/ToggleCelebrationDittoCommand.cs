// namespace CelebrationBoard.Application.Celebrations;

// public sealed class ToggleCelebrationDittoCommand : IRequest<UnitResult<Error>>
// {
//   public readonly long CelebrationId;
//   public readonly long UserId;

//   public ToggleCelebrationDittoCommand(long celebrationId, long userId)
//   {
//     CelebrationId = celebrationId;
//     UserId = userId;
//   }

//   internal sealed class ToggleCelebrationDittoCommandHandler : IRequestHandler<ToggleCelebrationDittoCommand, UnitResult<Error>>
//   {
//     private readonly CelebrationBoardContext _context;

//     public ToggleCelebrationDittoCommandHandler(CelebrationBoardContext context)
//     {
//       _context = context;
//     }

//     public async Task<UnitResult<Error>> Handle(ToggleCelebrationDittoCommand request, CancellationToken cancellationToken)
//     {
//       var user = this._context.Set<User>().Find(request.UserId);
//       if (user is null)
//         return UnitResult.Failure(Errors.General.NotFound(nameof(User), request.UserId));

//       var celebration = this._context.Set<Celebration>().Find(request.CelebrationId);
//       if (celebration is null)
//         return UnitResult.Failure(Errors.General.NotFound(nameof(Celebration), request.CelebrationId));

//       user!.ToggleCelebrationDitto(celebration!);
//       await this._context.SaveChangesAsync(cancellationToken);

//       return UnitResult.Success<Error>();
//     }
//   }
// }

