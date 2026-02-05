using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project
{
	public class Database
	{
		private List<Book> _books = new List<Book>();
		private List<Member> _members = new List<Member>();

		public async Task SaveBookAsync(Book book)
		{
			await Task.Delay(10);
			_books.Add(book);
		}

		public async Task SaveMemberAsync(Member member)
		{
			await Task.Delay(5);
			_members.Add(member);
		}

		public List<Book> GetBooks() => _books;
		public List<Member> GetMembers() => _members;

		public void ClearAll()
		{
			_books.Clear();
			_members.Clear();
		}
	}
}