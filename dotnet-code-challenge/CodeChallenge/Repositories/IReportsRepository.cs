using CodeChallenge.Models;
using System;

namespace CodeChallenge.Repositories
{
    public interface IReportsRepository
    {
        Employee GetById(String id);
    }
}
