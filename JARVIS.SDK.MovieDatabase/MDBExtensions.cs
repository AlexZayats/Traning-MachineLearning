using JARVIS.Core.Networking.Clients;
using JARVIS.Core.Networking.Extensions;
using JARVIS.SDK.MovieDatabase.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JARVIS.SDK.MovieDatabase
{
    public static class MDBExtensions
    {
        public const string BaseUrlV3 = "https://api.themoviedb.org/3/";

        public const string APIKeyParam = "api_key";
        public const string LanguageParam = "language";
        public const string PageParam = "page";
        public const string SortByParam = "sort_by";

        private static UriBuilder GetBuilder(string url)
        {
            var builder = new UriBuilder(BaseUrlV3 + url);
            builder.AddParameter(APIKeyParam, MDBSDK.ApiKey);
            builder.AddParameter(LanguageParam, MDBSDK.Language);
            return builder;
        }

        /// <summary>
        /// Get the system wide configuration information.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/configuration/get-api-configuration
        /// </remarks>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<Configuration> Configuration(this ApiClient client, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("configuration");
            return client.ApiGetAsync<Configuration>(builder.Uri, ct);
        }

        /// <summary>
        /// Get an up to date list of the officially supported movie certifications on TMDb.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/certifications/get-movie-certifications
        /// </remarks>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<CertificationsResponse> GetCertificationsMovie(this ApiClient client, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("certification/movie/list");
            return client.ApiGetAsync<CertificationsResponse>(builder.Uri, ct);
        }

        /// <summary>
        /// Get an up to date list of the officially supported TV show certifications on TMDb.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/certifications/get-tv-certifications
        /// </remarks>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<CertificationsResponse> GetCertificationsTV(this ApiClient client, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("certification/tv/list");
            return client.ApiGetAsync<CertificationsResponse>(builder.Uri, ct);
        }

        /// <summary>
        /// Discover movies by different types of data like average rating, number of votes, genres and certifications.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/discover/movie-discover
        /// </remarks>
        /// <param name="page">Specify the page of results to query.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<DiscoverResponse<Movie>> DiscoverMovie(this ApiClient client, int page = 1, string sortBy = null, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("discover/movie");
            builder.AddParameter(PageParam, page);
            if (!string.IsNullOrEmpty(sortBy))
            {
                builder.AddParameter(SortByParam, sortBy);
            }
            return client.ApiGetAsync<DiscoverResponse<Movie>>(builder.Uri, ct);
        }

        /// <summary>
        /// Discover TV shows by different types of data like average rating, number of votes, genres, the network they aired on and air dates.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/discover/tv-discover
        /// </remarks>
        /// <param name="page">Specify the page of results to query.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<DiscoverResponse<TVShow>> DiscoverTV(this ApiClient client, int page = 1, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("discover/tv");
            builder.AddParameter(PageParam, page);
            return client.ApiGetAsync<DiscoverResponse<TVShow>>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the list of official genres for movies.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/genres/get-movie-list
        /// </remarks>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<GenresResponse> GetGenresMovie(this ApiClient client, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("genre/movie/list");
            return client.ApiGetAsync<GenresResponse>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the list of official genres for TV shows.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/genres/get-tv-list
        /// </remarks>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<GenresResponse> GetGenresTV(this ApiClient client, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("genre/tv/list");
            return client.ApiGetAsync<GenresResponse>(builder.Uri, ct);
        }

        /// <summary>
        /// Get a companies details by id.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/companies/get-company-details
        /// </remarks>
        /// <param name="id">Company ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<Company> GetCompany(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"company/{id}");
            return client.ApiGetAsync<Company>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the details of a network.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/networks/get-network-details
        /// </remarks>
        /// <param name="id">Network ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<Company> GetNetwork(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"network/{id}");
            return client.ApiGetAsync<Company>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the primary person details by id.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/people/get-person-details
        /// </remarks>
        /// <param name="id">Person ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<Person> GetPerson(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"person/{id}");
            return client.ApiGetAsync<Person>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the movie credits for a person.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/people/get-person-movie-credits
        /// </remarks>
        /// <param name="id">Person ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<CreditsResponse<MovieCast>> GetPersonMovieCredits(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"person/{id}/movie_credits");
            return client.ApiGetAsync<CreditsResponse<MovieCast>>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the TV show credits for a person.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/people/get-person-tv-credits
        /// </remarks>
        /// <param name="id">Person ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<CreditsResponse<TVShowCast>> GetPersonTVShowCredits(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"person/{id}/tv_credits");
            return client.ApiGetAsync<CreditsResponse<TVShowCast>>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the movie and TV credits together in a single response.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/people/get-person-combined-credits
        /// </remarks>
        /// <param name="id">Person ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<CreditsResponse<MediaCast>> GetPersonCombinedCredits(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"person/{id}/combined_credits");
            return client.ApiGetAsync<CreditsResponse<MediaCast>>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the primary information about a movie.
        /// </summary>
        /// <param name="id">Movie ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<MovieDetails> GetMovie(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"movie/{id}");
            return client.ApiGetAsync<MovieDetails>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the primary TV show details by id.
        /// </summary>
        /// <param name="id">Movie ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<TVShowDetails> GetTV(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"tv/{id}");
            return client.ApiGetAsync<TVShowDetails>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the keywords that have been added to a movie.
        /// </summary>
        /// <param name="id">Movie ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<MovieKeywords> GetMovieKeywords(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"movie/{id}/keywords");
            return client.ApiGetAsync<MovieKeywords>(builder.Uri, ct);
        }

        /// <summary>
        /// Get the keywords that have been added to a TV show.
        /// </summary>
        /// <param name="id">TV show ID.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<TVShowKeywords> GetTVKeywords(this ApiClient client, int id, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder($"tv/{id}/keywords");
            return client.ApiGetAsync<TVShowKeywords>(builder.Uri, ct);
        }

        /// <summary>
        /// Search for companies.
        /// </summary>
        /// <remarks>
        /// https://developers.themoviedb.org/3/search/search-companies
        /// </remarks>
        /// <param name="query">Pass a text query to search. This value should be URI encoded.</param>
        /// <param name="ct">Task cancellation token.</param>
        public static Task<SearchResponse<Company>> SearchCompanies(this ApiClient client, string query, CancellationToken ct = default(CancellationToken))
        {
            var builder = GetBuilder("search/company");
            builder.AddParameter(PageParam, query);
            return client.ApiGetAsync<SearchResponse<Company>>(builder.Uri, ct);
        }
    }
}
