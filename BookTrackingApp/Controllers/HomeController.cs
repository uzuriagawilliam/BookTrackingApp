using BookTrackingApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Text;

namespace BookTrackingApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IBookRepository bookRepository, ILogger<HomeController> logger)
        {
            _bookRepository = bookRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            await LogIdentityInformation();
            return View();
        }
        [Authorize(Roles = "user, admin")]
        //[Authorize(Policy = "UserCanAddBook")]
        public async Task<IActionResult> Library(string status)
        {
            var OwnerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            if (OwnerId == null) { return View(); }/*TODO implement error message*/

            var books = await _bookRepository.GetBooks(OwnerId, status);
            return View(books);
        }

        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> FetchBook(string book, string author)
        {
            Rootobject rootobject = new Rootobject();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"https://www.googleapis.com/books/v1/volumes?q={@book}+inauthor:{@author}&maxResults=4&key=AIzaSyAwwGyMT-qHq2_mn_fLR6BoWTze9nsCa_E"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    try
                    {
                        rootobject = JsonConvert.DeserializeObject<Rootobject>(apiResponse);
                    }
                    catch (Exception ex)
                    {
                        rootobject.items = null;
                    }
                }
            }
            
            return View(rootobject);            
        }

        [Authorize]
        public async Task<IActionResult> Friends()
        {            
            List<FriendBooks> friendBooksList = new List<FriendBooks>();

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            int result = Int32.Parse(userid);
            var friends = await _bookRepository.GetFriends(result);

            string status = "";
            var books = await _bookRepository.GetBooks(userid, status);            
            

            foreach (var friend in friends)
            {
                FriendBooks friendBooks = new FriendBooks();
                friendBooks.Friend = friend;
                foreach(var book in books)
                {
                    if (book.FriendType != friend.FriendType)
                        continue;
                    friendBooks.Books.Add(book);
                }
                friendBooksList.Add(friendBooks);
            }

            return View(friendBooksList);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            UserProfile userProfile = new UserProfile();

            string userid = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            int result = Int32.Parse(userid);
            var friends = await _bookRepository.GetFriends(result);
            int totalFreiends = friends.Count();
            string name = User.Identity.Name;

            string status = "";
            var books = await _bookRepository.GetBooks(userid, status);
            int totalBooks = books.Count();

            userProfile.Id = result;
            userProfile.Name = name;
            userProfile.TotalFriends = totalFreiends;
            userProfile.TotalBooks = totalBooks;

            return View(userProfile);
        }

        [HttpPost]
        [Authorize(Roles = "user, admin")]        
        public async Task<IActionResult> Index([Bind("Image,Title,Author,Rating,Comment,Pages,Status,FriendType")] Book book)
        {
            if (!ModelState.IsValid)
            {
                return View(book);
            }
            book.OwnerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

            int len = book.Comment.Length;
            if(len > 200)
            {
                book.Comment.Substring(0, 198);
            }

            await _bookRepository.AddBook(book);

            
            return RedirectToAction(actionName: "AddNewBook", controllerName: "Home"); ; //TODO send the book title to the view
        }

        public IActionResult AddNewBook()
        {
            return View();
        }

        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> DeleteBook(int bookId)
        {

            await _bookRepository.RemoveBook(bookId);
           

            return RedirectToAction("Library");
        }
        [Authorize(Roles = "user, admin")]
        public async Task<IActionResult> Analitycs()
        {
            var ownerId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            string status = string.Empty;

            var analitics = await _bookRepository.GetBooks(ownerId, status);
            if (analitics == null)
            {
                return NotFound();
            }
            var totalBooks = analitics.Count();
            var alradyRead = analitics.Count(a => a.Status == "alrady-read");
            var willRead = analitics.Count(a => a.Status == "will-read-soon");
            var readingNow = analitics.Count(a => a.Status == "reading-now");
            var wantToRead = analitics.Count(a => a.Status == "want-to-read");

            TotalBooks BooksAnalitycs = new TotalBooks();

            BooksAnalitycs.wantToRead = wantToRead;
            BooksAnalitycs.willRead = willRead;
            BooksAnalitycs.Totalbooks = totalBooks;
            BooksAnalitycs.readingNow = readingNow;
            BooksAnalitycs.alradyRead = alradyRead;


            return View(BooksAnalitycs);
        }
        public async Task LogIdentityInformation()
        {
            var identityToken = await HttpContext
                .GetTokenAsync(OpenIdConnectParameterNames.IdToken);

            var userClaimsStringBuilder = new StringBuilder();
            foreach (var claim in User.Claims)
            {
                userClaimsStringBuilder.AppendLine(
                    $"Claim type: {claim.Type } - Claim value: {claim.Value}");
            }
            _logger.LogInformation($"Identity token & user claims: " + $"\n { identityToken} \n{userClaimsStringBuilder}");
        }
    }
}


public class Rootobject 
{
    public string kind { get; set; }
    public int totalItems { get; set; }
    public Item[] items { get; set; }
}

public class Item
{
    public string kind { get; set; }
    public string id { get; set; }
    public string etag { get; set; }
    public string selfLink { get; set; }
    public Volumeinfo volumeInfo { get; set; }
    public Saleinfo saleInfo { get; set; }
    public Accessinfo accessInfo { get; set; }
    public Searchinfo searchInfo { get; set; }
}

public class Volumeinfo
{
    public string title { get; set; }
    public string subtitle { get; set; }
    public string[] authors { get; set; }
    public string publisher { get; set; }
    public string publishedDate { get; set; }
    public string description { get; set; }
    public Industryidentifier[] industryIdentifiers { get; set; }
    public Readingmodes readingModes { get; set; }
    public int pageCount { get; set; }
    public string printType { get; set; }
    public string[] categories { get; set; }
    public string maturityRating { get; set; }
    public bool allowAnonLogging { get; set; }
    public string contentVersion { get; set; }
    public Panelizationsummary panelizationSummary { get; set; }
    public Imagelinks imageLinks { get; set; }
    public string language { get; set; }
    public string previewLink { get; set; }
    public string infoLink { get; set; }
    public string canonicalVolumeLink { get; set; }
    public int averageRating { get; set; }
    public int ratingsCount { get; set; }
}

public class Readingmodes
{
    public bool text { get; set; }
    public bool image { get; set; }
}

public class Panelizationsummary
{
    public bool containsEpubBubbles { get; set; }
    public bool containsImageBubbles { get; set; }
}

public class Imagelinks
{
    public string smallThumbnail { get; set; }
    public string thumbnail { get; set; }
}

public class Industryidentifier
{
    public string type { get; set; }
    public string identifier { get; set; }
}

public class Saleinfo
{
    public string country { get; set; }
    public string saleability { get; set; }
    public bool isEbook { get; set; }
    public Listprice listPrice { get; set; }
    public Retailprice retailPrice { get; set; }
    public string buyLink { get; set; }
    public Offer[] offers { get; set; }
}

public class Listprice
{
    public float amount { get; set; }
    public string currencyCode { get; set; }
}

public class Retailprice
{
    public float amount { get; set; }
    public string currencyCode { get; set; }
}

public class Offer
{
    public int finskyOfferType { get; set; }
    public Listprice1 listPrice { get; set; }
    public Retailprice1 retailPrice { get; set; }
}

public class Listprice1
{
    public int amountInMicros { get; set; }
    public string currencyCode { get; set; }
}

public class Retailprice1
{
    public int amountInMicros { get; set; }
    public string currencyCode { get; set; }
}

public class Accessinfo
{
    public string country { get; set; }
    public string viewability { get; set; }
    public bool embeddable { get; set; }
    public bool publicDomain { get; set; }
    public string textToSpeechPermission { get; set; }
    public Epub epub { get; set; }
    public Pdf pdf { get; set; }
    public string webReaderLink { get; set; }
    public string accessViewStatus { get; set; }
    public bool quoteSharingAllowed { get; set; }
}

public class Epub
{
    public bool isAvailable { get; set; }
    public string acsTokenLink { get; set; }
}

public class Pdf
{
    public bool isAvailable { get; set; }
    public string acsTokenLink { get; set; }
}

public class Searchinfo
{
    public string textSnippet { get; set; }
}
