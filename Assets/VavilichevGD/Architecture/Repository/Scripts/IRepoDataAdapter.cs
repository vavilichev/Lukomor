namespace VavilichevGD.Architecture.StorageSystem {
    public interface IRepoDataAdapter {
        RepoData Adapt(RepoData oldRepoData);
    }
}