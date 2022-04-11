# TreeView et FileChooser

Réalisé par Louis Forestier et Clémentine Guillot.
La version de l'éditeur Unity utilisé est : Unity 2020.3.23f1
La version de Unity Hub utilisé est : 3.1.0

Les scripts sont documentés, leur description, dans ce readme, est donc succincte. 

## Widget TreeView

### TreeView
Le GameObject TreeView est une ScrollView.
Il a un script TreeView qui récupère le GameObject Root via l'inspecteur. 
Le GameObject Root est un l'objet de ViewPort (élément visible d'une scrollview) qui contient un Vertical Layout. 
Il est le premier élément de l'arbre, il n'est cependant jamais visible par l'utilisateur. 
L'ancrage des éléments dans ce vertical layout se fait en haut et avec la plus grande largeur possible. 
Il contôle la taille de ses enfants pour éviter qu'ils ne se chevauche.

Le script TreeView a donc un attribut public root de type GameObject. 
Il a également des méthodes utiles pour la manipulation de l'arbre.
Il y a notamment une méthode qui permet d'ajouter des éléments. Les éléments sont des TreeItem.

### TreeItem
TreeItem est un GameObject avec un vertical layout et un script TreeItem. Il a également deux fils, Node et Children.
Ces deux fils sont récupéré par le script TreeItem ainsi que le Text fils de Node.

Node est un bouton qui a deux fils, Text et Arrow. Il correspond à une ligne dans le treeview. Ligne qui est donc cliquable.
Il a un horizontal layout pour placer Arrow et Text correctement. 
Arrow est le bouton flèche qui change d'orientation et affiche ou non les enfants contenus dans Children.
Text est un texte qui affiche le nom du noeud.

Children est un noeud vide qui a un vertical layout. Il est le GameObject père des TreeItem fils.
Le vertical layout a un padding à gauche qui permet d'indenter les fils de manière récursive.

Le script TreeItem a des méthodes pour la manipulation des treeitems. La méthode ChildrenVisible est celle qui est
appelée par le bouton flèche. Elle permet de tourner le bouton et de changer la visibilité des fils.

## Widget FileChooser

### FileChooser
Le GameObject FileChooser a un script FileChooser et un vertical layout. 
Il a deux fils, TopPanel et MiddlePanel. 
TopPanel a un layout element et un horizontal layout. Il correspond à la barre du haut. Ses fils sont un bouton return qui 
remonte au répertoire parent, un inputfield en read only pour afficher le répertoire courant, et un bouton quit qui ferme 
le filechooser. 
MiddlePanel a un layout element et un horizontal layout. Il affiche l'arborescence des fichiers dans un fils TreeView, et 
le contenu du répertoire courant dans un fils FilesContainer.

Le script FileChooser génère les noeuds de la TreeView et les items du filecontainer en fonction des fichier présents sur 
la machine. Il a une méthode statique ChooseFile qui permet une utilisation simple du widget. Il suffit d'appeler cette 
méthode avec une fonction en paramètre. Cette fonction déterminera l'action d'un clic sur un fichier dans le FilesContainer.
Elle doit prendre en paramètre un String, qui sera le chemin absolu du fichier sélectionné. 

### FilesContainer
FilesContainers est une scrollview avec un fils ViewPort. Content est le fils de ViewPort, il a un vertical layout. 
Il sera le parent de tous les FileItem. 

### FileItem
FileItem est un bouton avec un script FileItem et un horizontal layout.
Il affiche l'icône de fichier ou de répertoire selon la nature de l'élément, et le nom de l'élément.

Le script FileItem permet de modifier le texte et l'icône de celui-ci, ainsi que de récupérer l'événement OnClick pour
lui ajouter des listeners.

### FileChooserCanvas
Il s'agit d'un Canvas qui contient le préfab FileChooser. Il est dans le répertoire Resources pour être accessible avec la 
méthode Resources.Load de Unity. Il est notamment instancié dans la méthode statique ChooseFile de FileChooser. 

## Notre application
Nous avons créé une application pour montrer l'utilisation de nos préfab.
L'application permet de changer la texture des astres du système solaire.
On y voit un système solaire et une treeview contenant le nom des tous les astres visibles.
Cliquer sur les flèches des TreeItem permet de déplier l'arborescence.
Lorsque l'on clique sur le noeud contenant le nom d'un des astres, par exemple la Terre, un filechooser s'ouvre.
On peut alors choisir une image, seul le format jpg est reconnu. L'image sera alors appliquée
à la Terre.


