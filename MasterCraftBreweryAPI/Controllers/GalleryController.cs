using AutoMapper;
using Core.DTO;
using Core.ErrorHandling;
using Core.Managers;
using Core.Util;
using MasterCraftBreweryAPI.ErrorHandling;
using MasterCraftBreweryAPI.Mapper;
using MasterCraftBreweryAPI.Util;
using MasterCraftBreweryAPI.Wrapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MasterCraftBreweryAPI.Controllers
{
    [Route("galleries")]
    [ApiController]
    [Authorize(AuthenticationSchemes = Authentication.Authentication.ApiKeyScheme)]
    public class GalleryController : MasterCraftBreweryControllerBase
    {
        private readonly IGalleryManager galleryManager;

        public GalleryController(IGalleryManager galleryManager, IApiKeyManager apiKeyManager, IMapper mapper) : base(apiKeyManager, mapper)
            => (this.galleryManager) = (galleryManager);

        /// <summary>
        /// Adds new gallery to the database with gallery name and media files
        /// </summary>
        /// <param name="galleryPostWrapper">The gallery information with media files data</param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Add([FromBody] GalleryPostWrapper galleryPostWrapper)
        {
            ResultMessage<GalleryBaseDTO> result = await galleryManager.Add(mapper.Map<GalleryBaseDTO>(galleryPostWrapper));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="galleryId">Unique identifier of gallery</param>
        /// <param name="images">Images</param>
        /// <returns></returns>
        [HttpPut("{galleryId}/images")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> AddImages([FromRoute] int galleryId, List<IFormFile> images)
        {
            List<BasicFileInfo> basicFiles = images.ConvertAll(image => image.AsBasicFileInfo());
            ResultMessage<GalleryDTO> result = await galleryManager.AddImagesToGallery(galleryId, basicFiles);
            return HttpResultMessage.FilteredResult(result);
        }


        /// <summary>
        /// Updates gallery with media files
        /// </summary>
        /// <param name="galleryPutWrapper">The gallery put information with media file data</param>
        /// <returns></returns>
        [HttpPut]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Update([FromBody] GalleryPutWrapper galleryPutWrapper)
        {
            ResultMessage<GalleryDTO> result = await galleryManager.Update(mapper.Map<GalleryDTO>(galleryPutWrapper));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Deletes gallery and media files from data base and file storage.
        /// </summary>
        /// <param name="galleryId">Unique identifier of the gallery</param>
        /// <returns></returns>
        [HttpDelete("{galleryId}")]
        [Authorize(Policy = "TokenRequired")]
        public async Task<ActionResult> Delete([FromRoute] int galleryId)
        {
            ResultMessage<bool> result = await galleryManager.Delete(galleryId);
            return HttpResultMessage.FilteredResult<GalleryDeleteWrapper, bool>(result);
        }

        /// <summary>
        /// Returns gallery by unique identifier that is sent, and downloads 
        /// all media files that are in that gallery
        /// </summary>
        /// <param name="galleryId">Unique identifier of the gallery</param>
        /// <param name="dimensions">Image dimensions</param>
        /// <returns></returns>
        [HttpGet("{galleryId}")]
        public async Task<ActionResult> GetById([FromRoute] int galleryId, [FromQuery] ThumbnailDimensionsWrapper dimensions)
        {
            ResultMessage<DetailedGalleryDTO> result = await galleryManager.GetById(galleryId, dimensions?.ToCoreObject(mapper));
            return HttpResultMessage.FilteredResult(result);
        }

        /// <summary>
        /// Returns informations about all galleries and returns their thumbnail images.
        /// </summary>
        /// <param name="dimensions">Image dimensions</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] ThumbnailDimensionsWrapper dimensions)
        {
            ResultMessage<List<GalleryWithThumbnailDTO>> result = await galleryManager.GetAll(dimensions?.ToCoreObject(mapper));
            return HttpResultMessage.FilteredResult(result);
        }
    }
}
