using System.Reflection.Metadata;
using Marquee.Application.Interfaces;
using Marquee.Application.Interfaces.Repositories;
using Marquee.Application.Interfaces.Services;
using Marquee.Domain.Entities;

namespace Marquee.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _userRepository.GetAllAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        var existingUser =  await _userRepository.GetByIdAsync(user.Id, cancellationToken);
        if (existingUser == null)
        {
            throw new KeyNotFoundException($"User with ID {user.Id} was not found.");
        }

        existingUser.DisplayName = user.DisplayName;
        existingUser.Bio =  user.Bio;
        existingUser.ProfileImageUrl = user.ProfileImageUrl;
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(id, cancellationToken);
        
        if (user == null)
        {
            throw new KeyNotFoundException($"User with ID {id} was not found.");
        }
        
        _userRepository.Delete(user);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}