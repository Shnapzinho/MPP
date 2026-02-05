using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project
{
	public class LibraryService
	{
		private readonly Database _db;

		public LibraryService(Database db) => _db = db;

		public async Task RegisterMemberAsync(string name)
		{
			if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name cannot be empty");
			var member = new Member { Id = _db.GetMembers().Count + 1, Name = name };
			await _db.SaveMemberAsync(member);
		}

		public async Task AddBookAsync(string title, string author)
		{
			if (string.IsNullOrEmpty(title)) throw new ArgumentException("Title is required");
			var book = new Book
			{
				Id = _db.GetBooks().Count + 1,
				Title = title,
				Author = author,
				IsBorrowed = false
			};
			await _db.SaveBookAsync(book);
		}

		public void BorrowBook(int bookId, int memberId)
		{
			var book = _db.GetBooks().FirstOrDefault(b => b.Id == bookId);
			var member = _db.GetMembers().FirstOrDefault(m => m.Id == memberId);
			if (book == null) throw new Exception("Book not found");
			if (member == null) throw new Exception("Member not found");
			if (book.IsBorrowed) throw new InvalidOperationException("Book is already borrowed");
			if (member.BorrowedCount >= Member.MaxBooks) throw new InvalidOperationException("Book limit exceeded");
			book.IsBorrowed = true;
			book.BorrowedByMemberId = memberId;
			member.BorrowedCount++;
		}

		public List<Book> SearchBooks(string query)
		{
			return _db.GetBooks()
				.Where(b => b.Title.Contains(query) || b.Author.Contains(query))
				.ToList();
		}
	}
}