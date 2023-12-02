using BuildingBlocks.Application.UploadImage;
using BuildingBlocks.Domain;
using Catalog.Domain.BookImages;
using Catalog.Domain.Books;
using Catalog.Domain.Libraries;
using Catalog.Domain.Owners;
using Catalog.Infrastructure.Configuration.Commands;

namespace Catalog.API.Application.Books.UploadBookImage
{
    public class UploadBookImageCommandHandler : ICommandHandler<UploadBookImageCommand, Result>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUploadImageService _uploadImageService;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IImageRepository _imageRepository;
        public UploadBookImageCommandHandler(IBookRepository bookRepository, IUploadImageService uploadImageService, IOwnerRepository ownerRepository, ILibraryRepository libraryRepository, IImageRepository imageRepository)
        {
            _bookRepository = bookRepository;
            _uploadImageService = uploadImageService;
            _ownerRepository = ownerRepository;
            _libraryRepository = libraryRepository;
            _imageRepository = imageRepository;
        }

        public async Task<Result> Handle(UploadBookImageCommand request, CancellationToken cancellationToken)
        {
            Owner owner = await _ownerRepository.GetByUsername(request.Username);
            if (owner == null)
            {
                return Result.Failure("This owner does not exist.");
            }

            Book book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book == null)
            {
                return Result.Failure("This book does not exist");
            }

            bool ownerOfLibrary = _libraryRepository.OwnerOfLibrary(book.LibraryId, owner.OwnerId);
            if (!ownerOfLibrary)
            {
                return Result.Failure("Only owner of library can upload image of book.");
            }

            BuildingBlocks.Domain.Results.Image image = _uploadImageService.UploadImage(request.ImageFile, request.WwwRootPath, request.Folder);
            Image imageOfBook = (Image)image;

            book.ImageId = imageOfBook.ImageId;
            book.Image = imageOfBook;
            _bookRepository.UpdateBook(book);
            await _imageRepository.AddAsync(imageOfBook);

            return Result.Success(imageOfBook);
        }
    }
}
