using UnityEngine;

public class AjouterVecteur : MonoBehaviour
{
    //Structure pour les données de l'addition de vecteurs
    [System.Serializable]
    public struct additionStruct 
    {
        public Vector2 vectorOne, vectorTwo;
        public bool activer;
    }

    //Structure pour les données de la soustraction de vecteurs
    [System.Serializable]
    public struct substractionStruct 
    {
        public Vector2 vectorOne, vectorTwo;
        public bool activer;
    }

    //Structure pour la multiplication scalaire
    [System.Serializable]
    public struct scalaireMultiInput 
    {
        public Vector2 vector;
        public float multiplicateur;
        public bool activer;
    }

    //Toutes les variables, les Serialize apparaissent dans l'éditeur
    [SerializeField]additionStruct additionVectoriel;
    [SerializeField]substractionStruct soustractionVectoriel;
    [SerializeField]scalaireMultiInput multiplicationScalaire;
    [SerializeField]float produitScalaire, normeVecteur;
    [SerializeField]Vector2 normalisationVecteur;
    [SerializeField]bool recover;
    Vector2 vectorOne, vectorTwo;
    Vector3 vectorResult;
    bool inMovement;
    Rigidbody rigibody, rigibodyChildOne, rigibodyChildTwo;
    AjouterVecteur childOne, childTwo;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        produitScalaire = normeVecteur = 0;
        additionVectoriel.vectorOne = additionVectoriel.vectorTwo = soustractionVectoriel.vectorOne = soustractionVectoriel.vectorTwo = multiplicationScalaire.vector = normalisationVecteur = new Vector2(0,0);
        vectorResult = new Vector3(0,0,0);
        additionVectoriel.activer = soustractionVectoriel.activer = multiplicationScalaire.activer = recover = inMovement = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Addition
        if (additionVectoriel.activer)
        {
            Ajouter();
        }
        //Soustraction
        else if (soustractionVectoriel.activer)
        {
            Soustraire();
        }
        //Multiplication
        else if (multiplicationScalaire.activer)
        {
            MultiplicationScalaire();
        }
        //Récupérer les cubes à leur place initiale
        if (recover)
        {
            Recover();
        }
        //Fonction pour faire fonctionner l'animation
        if (inMovement)
        {
            InMovement();
        }
    }

    //Fonction pour l'addition vectorielle
    public void Ajouter()
    {
        //Définition des variables pour faciliter l'utilisation
        produitScalaire = normeVecteur = 0;
        vectorOne = additionVectoriel.vectorOne;
        vectorTwo = additionVectoriel.vectorTwo;
        //Failsafe pour s'assurer que tout est loadé, j'ai essayé de le mettre dans start et ça ne marchait pas donc on utilise dans update
        loadEverything();
        //Calcule du vecteur résultant
        vectorResult = new Vector3((vectorOne.x + vectorTwo.x), (vectorOne.y + vectorTwo.y), 0);
        //Comme toujours, on vérifie s'il y a des childs, c'est le moyen de vérifier si c'est le cube mauve ou un des deux autres (C'est le même script pour les 3)
        if (gameObject.transform.childCount > 0)
        {
            //On transmet le vecteur aux 2 autres
            childOne.additionVectoriel.vectorOne = vectorOne;
            childTwo.additionVectoriel.vectorOne = vectorTwo;
            childOne.additionVectoriel.activer = true;
            childTwo.additionVectoriel.activer = true;
        }
        //Calcul des autres variables pour affichage
        produitScalaire = ((vectorOne.x * vectorTwo.x) + (vectorOne.y * vectorTwo.y));
        normeVecteur = Mathf.Sqrt((vectorResult.x * vectorResult.x)+(vectorResult.y * vectorResult.y));
        normalisationVecteur = new Vector2((vectorResult.x/normeVecteur),(vectorResult.y/normeVecteur));
        //On initialise le mouvement
        InitialiseMove();
    }

    public void Soustraire()
    {
        //Définition des variables pour faciliter l'utilisation
        produitScalaire = normeVecteur = 0;
        vectorOne = soustractionVectoriel.vectorOne;
        vectorTwo = soustractionVectoriel.vectorTwo;
        //Failsafe pour s'assurer que tout est loadé, j'ai essayé de le mettre dans start et ça ne marchait pas donc on utilise dans update
        loadEverything();
        vectorResult = new Vector3((vectorOne.x - vectorTwo.x), (vectorOne.y - vectorTwo.y), 0);
        //Comme toujours, on vérifie s'il y a des childs, c'est le moyen de vérifier si c'est le cube mauve ou un des deux autres (C'est le même script pour les 3)
        if (gameObject.transform.childCount > 0)
        {
            //On transmet le vecteur aux 2 autres
            childOne.soustractionVectoriel.vectorOne = vectorOne;
            childTwo.soustractionVectoriel.vectorTwo = vectorTwo;
            childOne.soustractionVectoriel.activer = true;
            childTwo.soustractionVectoriel.activer = true;
        }
        //Calcul des autres variables pour affichage
        produitScalaire = ((vectorResult.x*vectorResult.x) + (vectorResult.y*vectorResult.y));
        normeVecteur = Mathf.Sqrt((vectorResult.x * vectorResult.x)+(vectorResult.y * vectorResult.y));
        normalisationVecteur = new Vector2((vectorResult.x/normeVecteur),(vectorResult.y/normeVecteur));
        //On initialise le mouvement
        InitialiseMove();
    }

    public void MultiplicationScalaire()
    {
        produitScalaire = normeVecteur = 0;
        //Failsafe pour s'assurer que tout est loadé, j'ai essayé de le mettre dans start et ça ne marchait pas donc on utilise dans update
        loadEverything();
        vectorResult = new Vector3((multiplicationScalaire.vector.x * multiplicationScalaire.multiplicateur), (multiplicationScalaire.vector.y * multiplicationScalaire.multiplicateur), 0);
        //Comme toujours, on vérifie s'il y a des childs, c'est le moyen de vérifier si c'est le cube mauve ou un des deux autres (C'est le même script pour les 3)
        if (gameObject.transform.childCount > 0)
        {
            //On transmet le vecteur au cube rouge
            childOne.additionVectoriel.vectorOne = multiplicationScalaire.vector;
            childOne.additionVectoriel.activer = true;
        }
        //Calcul des autres variables pour affichage
        produitScalaire = ((multiplicationScalaire.vector.x * multiplicationScalaire.vector.x) + (multiplicationScalaire.vector.y * multiplicationScalaire.vector.y)) * multiplicationScalaire.multiplicateur;
        normeVecteur = Mathf.Sqrt((vectorResult.x * vectorResult.x)+(vectorResult.y * vectorResult.y));
        normalisationVecteur = new Vector2((vectorResult.x/normeVecteur),(vectorResult.y/normeVecteur));
        //On initialise le mouvement
        InitialiseMove();
    }

    public void InitialiseMove()
    {
        Recover();
        //Failsafe pour s'assurer que tout est loadé, j'ai essayé de le mettre dans start et ça ne marchait pas donc on utilise dans update
        loadEverything();
        rigibody.AddForce(vectorResult * 50);
        additionVectoriel.activer = false;
        soustractionVectoriel.activer = false;
        multiplicationScalaire.activer = false;
        inMovement = true;
    }

    public void Recover()
    {
        loadEverything();
        //Do it for the result cube
        rigibody.transform.position = new Vector3 (0, 0, 0);
        rigibody.transform.rotation = new Quaternion (0, 0, 0, 0);
        rigibody.linearVelocity = new Vector3 (0, 0, 0);
        rigibody.angularVelocity = new Vector3 (0, 0, 0);

        //Do it for the other two
        if (gameObject.transform.childCount > 0)
        {
            childOne.recover = childTwo.recover = true;
        }
        recover = false;
    }

    //Script qui organise le mouvement des cubes
    public void InMovement()
    {
        loadEverything();
        //On vérifie si le cube doit encore a atteint sa destination
        if (rigibody.transform.position == vectorResult)
            {
                //Si oui, on arrête tout le mouvement
                rigibody.transform.position = vectorResult;
                rigibody.linearVelocity = new Vector3 (0, 0, 0);
                rigibody.angularVelocity = new Vector3 (0, 0, 0);
                inMovement = false;
                //On reset nos vecteurs, juste au cas où, pour éviter des soucis
                if (gameObject.transform.childCount == 0)
                {
                    additionVectoriel.vectorOne = additionVectoriel.vectorTwo = soustractionVectoriel.vectorOne = soustractionVectoriel.vectorTwo = multiplicationScalaire.vector = new Vector2(0,0);
                    vectorResult = new Vector3(0,0,0);
                }
            }
    }

    //Failsafe pour s'assurer que tout est loadé, j'ai essayé de le mettre dans start et ça ne marchait pas donc on utilise dans update
    public void loadEverything()
    {
        rigibody = GetComponent<Rigidbody>();
        //Comme toujours, on vérifie s'il y a des childs, c'est le moyen de vérifier si c'est le cube mauve ou un des deux autres (C'est le même script pour les 3)
        if (gameObject.transform.childCount > 0)
        {
            //On va également chercher les composantes des "child"
            childOne = gameObject.transform.GetChild(0).gameObject.GetComponent<AjouterVecteur>();
            childTwo = gameObject.transform.GetChild(1).gameObject.GetComponent<AjouterVecteur>();
            rigibodyChildOne = gameObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
            rigibodyChildTwo = gameObject.transform.GetChild(1).gameObject.GetComponent<Rigidbody>();
        }

    }
        
}
