using UnityEngine;
using VavilichevGD.Architecture.StorageSystem;

namespace VavilichevGD.Architecture.Example {
	public sealed class DummyRepository : Repository {

		#region CONSTANTS

		private const string ID = "DUMMY_REPOSITORY";

		#endregion

		public override string id => ID;

		public DummyRepoEntity repoEntity { get; private set; }


		protected override void Initialize() {
			base.Initialize();
			var repoData = Storage.GetRepoData(this.id, this.GetRepoDataDefault());
			this.repoEntity = repoData.GetEntity<DummyRepoEntity>();
		}


		public override RepoData GetRepoData() {
			return new RepoData(ID, this.repoEntity, version);
		}

		public override RepoData GetRepoDataDefault() {
			return new RepoData(this.id, new DummyRepoEntity(), version);
		}

		public override void UploadRepoData(RepoData repoData) {
			this.repoEntity = repoData.GetEntity<DummyRepoEntity>();
		}

		protected override void OnInitialized() {
			Debug.Log("DUMMY REPO LOADED: \n" + this.repoEntity.ToJson());
		}
	}
}