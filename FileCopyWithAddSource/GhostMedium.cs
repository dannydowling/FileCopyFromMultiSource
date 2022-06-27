using System;

    public class GhostMedium : objectModel
    {

    // first we set up the idea of loading the medium to be tested.
        private LoadStatus status;
        private readonly Func<objectModel> load;

        public bool IsGhost => status == LoadStatus.GHOST;
        public bool IsLoaded => status == LoadStatus.LOADED;

        public GhostMedium(Func<objectModel> load) : base()
        {
            this.load = load;
            status = LoadStatus.GHOST;
        }

    // then we have to lazily load the medium so that two instances aren't created.
        public object obj 
        {
            get
            {
                Load();

                return base.obj;
            }
            set
            {
                Load();

                base.obj = value;
            }
        }
            
        public void Load()
        {
            if (IsGhost)
            {
                status = LoadStatus.LOADING;

                var objectLoad = load();
                base.obj = objectLoad.obj;
                status = LoadStatus.LOADED;
            }
        }

    enum LoadStatus { GHOST, LOADING, LOADED };
}

