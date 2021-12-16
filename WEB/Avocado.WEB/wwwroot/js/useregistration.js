class Form {
	constructor() {
		this.UserName;
		this.Password;
		this.Name;
		this.LastName;
		this.PhoneNumber;
		this.PostalCode;
		this.StreetAddress;
		this.City;
		this.State
	}
}
document.addEventListener('DOMContentLoaded', (e) => {

	let form = document.getElementById('form');
	let elements = form.elements;

	let formulario = new Form();

	form.addEventListener('submit', (e) => {

		e.preventDefault();
		e.stopPropagation();

		formulario.userName = elements.userName.value;
		formulario.password = elements.password.value;
		formulario.Name = elements.Name.value;
		formulario.LastName = elements.LastName.value;
		formulario.StreetAddress = elements.address.value;
		formulario.PostalCode = elements.PC.valueAsNumber;
		formulario.PhoneNumber = elements.PhoneNumber.value;
		formulario.City = elements.City.value;
		formulario.State = elements.State.value;

		if (validate()) {
			postForm(formulario);
		}

		console.log(formulario);
	});

	function validate() {

		if (form.userName.value === "") {
			alert('Username is required!');
			return false;
		}
		if (form.password.value === "") {
			alert('Password is required!');
			return false;
		}
		else if (form.password.value.length <= 5) {
			alert('Password is not long enough!');
			return false;
		}
		if (form.PhoneNumber.value != "" && !parseInt(form.PhoneNumber.value)) {
			alert('Only numbers in phone number!');
			return false;
		}
		return true;
	}

});

function postForm(formulario) {
	fetch('https://localhost:44363/users/register', {
		headers: {
			'Content-type': 'application/json'
		},
		method: 'POST',
		body: JSON.stringify(formulario)
	}).then((response) =>
		response.json())
		.then((json) =>
			alert(json));
	window.location.href = "https://localhost:44348/account/login";
}