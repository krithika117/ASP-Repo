function initCategories(categoryObject, categorySel, subcategorySel) {
	var categories = Object.keys(categoryObject);

	// populate cateogories
	for (var i = 0; i < categories.length; i++) {
		categorySel.options[i + 1] = new Option(categories[i], categories[i]);
	}

	categorySel.onchange = function () {
		// empty sub-category dropdown
		subcategorySel.length = 1;
		// display correct values
		var z = categoryObject[this.value];
		for (var i = 0; i < z.length; i++) {
			subcategorySel.options[subcategorySel.options.length] = new Option(z[i], z[i]);
		}
	}
}