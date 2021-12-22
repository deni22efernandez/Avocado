$('#details').on('show.bs.modal', function (event) {
	var button = $(event.relatedTarget)
	var number = button.data('value')
	var date = button.data('date')
	var total = button.data('total')
	var orderStatus = button.data('orderstatus')
	var paymentStatus = button.data('paymentstatus')
	var carrier = button.data('carrier')
	var trackingNmb = button.data('trackingnmb')
	var modal = $(this)
	modal.find('.order-number').text(number)
	modal.find('.order-date').text(date)
	modal.find('.order-total').text('$' + total)
	modal.find('.order-orderStatus').text(orderStatus)
	modal.find('.order-paymentStatus').text(paymentStatus)
	modal.find('.order-carrier').text(carrier)
	modal.find('.order-trackingNmb').text(trackingNmb)
})

document.addEventListener('DOMContentLoaded', () => {
	const form = document.querySelector('#form');
	form.addEventListener('submit', (e) => {
		e.preventDefault();
		e.stopPropagation();

		if (validate()) {
			class OrderHeader {
				constructor() {
					this.Id;
					this.carrier;
					this.trackingNumber
				}
			}
			const Details = new OrderHeader();

			const elements = form.elements;

			Details.Id = parseInt(elements.ordernumber.value);
			Details.carrier = elements.carrier.value;
			Details.trackingNumber = elements.tracking.value;
			console.log(Details);

			return fetch('https://localhost:44363/orderHeaders', {
				headers: {
					'Content-type': 'application/json'
				},
				body: JSON.stringify(Details),
				method: 'PATCH'
			}).then(response =>
				response.json()
			).then(data =>
				alert(JSON.stringify(data.value))
			).catch(console.log.bind(console));

		}
	});
});

function validate() {
	const carrier = document.querySelector("#carrier");
	const tracking = document.querySelector("#tracking");

	if (carrier.value == null || carrier.value == "") {
		alert('carrier is required!');
		return false;
	}
	if (tracking.value == null || tracking.value == "") {
		alert('tracking is required!');
		return false;
	}
	return true;
}