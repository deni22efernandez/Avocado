class Category {
    constructor() {
        this.Name;
    }
}
document.addEventListener('DOMContentLoaded', () => {

    const category = new Category();
    const CategoryForm = document.querySelector('#form');
    const elements = CategoryForm.elements;

    CategoryForm.addEventListener('submit', (e) => {

        e.preventDefault();
        e.stopPropagation();

        category.Name = elements.categoryName.value;
       // console.log(category);

        if (ValidateInput()) {
            Create(category);
        }
    });
})

function Create(category) {
       return fetch("https://localhost:44363/categories/", {
        headers: {
            'Content-type': 'application/json'
        },
        body: JSON.stringify(category),
        method: 'POST'
       }).then((response) => {
           response.json()
       }).then((json) =>
        alert(`Success: ${JSON.stringify(json)}  was created`)
    ).catch(console.log.bind(console));
}

function ValidateInput() {
    let inputName = document.querySelector('#categoryName');
    let alerta = document.querySelector('#validation');

    if (inputName.value === "") {
        alerta.innerHTML = 'Category name is required!';
        return false;
    }

    if (parseInt(inputName.value)) {
        alerta.innerHTML = 'Ivalid category name!, only letters are allowed...';
        return false;
    }
    inputName = "";
    alerta.innerHTML = "";
    return true;
}