<template>
    <div id="product">
        <v-app id="inspire">
            <v-row>
                <v-col cols="12" sm="6" md="4">
                    <v-text-field append-icon="search"
                                  label="Filter"
                                  single-line
                                  hide-details
                                  @input="filterSearch"></v-text-field>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-select :items="categories"
                              item-text="name"
                              item-value="id"
                              label="Categories"
                              @change="filterCategories"></v-select>
                </v-col>
                <v-col cols="12" sm="6" md="4">
                    <v-select :items="suppliers"
                              item-text="name"
                              item-value="id"
                              label="Suppliers"
                              @change="filterSuppliers"></v-select>
                </v-col>
            </v-row>

            <v-data-table :headers="headers"
                          :items="products"
                          sort-by="name"
                          :search="filters"
                          :custom-filter="customFilter"
                          class="elevation-1">
                <template v-slot:top>
                    <v-toolbar flat color="white">
                        <v-toolbar-title>Products</v-toolbar-title>
                        <v-divider class="mx-4"
                                   inset
                                   vertical></v-divider>
                        <v-spacer></v-spacer>
                        <v-dialog v-model="dialog" max-width="500px">
                            <template v-slot:activator="{ on, attrs }">
                                <v-btn color="primary"
                                       dark
                                       class="mb-2"
                                       v-bind="attrs"
                                       v-on="on">New Item</v-btn>
                            </template>
                            <v-card>
                                <v-card-title>
                                    <span class="headline">{{ formTitle }}</span>
                                </v-card-title>

                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <v-col cols="12" sm="12" md="12">
                                                <v-text-field v-model="editedItem.name" label="Name" :error-messages="modelstate['Name']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="6">
                                                <v-select v-model="editedItem.supplierId"
                                                          :items="suppliers"
                                                          item-text="name"
                                                          item-value="id"
                                                          :error-messages="modelstate['SupplierId']"
                                                          label="Supplier"></v-select>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="6">
                                                <v-select v-model="editedItem.categoryId"
                                                          :items="categories"
                                                          item-text="name"
                                                          item-value="id"
                                                          :error-messages="modelstate['CategoryId']"
                                                          label="Category"></v-select>
                                            </v-col>

                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.quantityPerUnit" type="number" label="Quantity Per Unit" :error-messages="modelstate['QuantityPerUnit']"></v-text-field>
                                            </v-col>

                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field prefix="ZAR" @keypress="currencyValidate" v-model="editedItem.unitPrice" type="number" label="Unit Price" :error-messages="modelstate['UnitPrice']"></v-text-field>
                                            </v-col>

                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.unitsInStock" type="number" label="Units In Stock" :error-messages="modelstate['UnitsInStock']"></v-text-field>
                                            </v-col>

                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.reorderLevel" type="number" label="Reorder Level" :error-messages="modelstate['ReorderLevel']"></v-text-field>
                                            </v-col>

                                            <v-col cols="12" sm="12" md="12">
                                                <v-checkbox v-model="editedItem.discontinued"
                                                            label="Discontinued"></v-checkbox>
                                            </v-col>

                                        </v-row>
                                    </v-container>
                                </v-card-text>

                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
                                    <v-btn color="blue darken-1" text @click="save">Save</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>
                    </v-toolbar>
                </template>
                <template v-slot:item.imageUrl="{ item }">
                    <div v-if="item.imageUrl">
                        <v-avatar>
                            <img :src="item.imageUrl" :alt="item.name">
                        </v-avatar>
                    </div>
                    <div v-else>
                        <v-avatar color="indigo">
                            <v-icon dark>mdi-signature-image</v-icon>
                        </v-avatar>
                    </div>
                </template>
                <template v-slot:item.actions="{ item }">
                    <v-icon small
                            class="mr-2"
                            @click="editItem(item)">
                        mdi-pencil
                    </v-icon>
                    <v-icon small
                            @click="deleteItem(item)">
                        mdi-delete
                    </v-icon>
                </template>
                <template v-slot:no-data>
                    <v-btn color="primary" @click="search">Reset</v-btn>
                </template>
            </v-data-table>
        </v-app>
    </div>

</template>


<script>
    export default {
        name: "product-component",
        data() {
            return {
                filters: {
                    search: '',
                    category: '',
                    supplier: '',
                },
                products: [],
                categories: [],
                suppliers: [],
                modelstate: {},
                dialog: false,
                headers: [
                    {
                        text: 'Id',
                        align: 'start',
                        sortable: false,
                        value: 'id',
                    },
                    { text: 'Name', value: 'name' },
                    { text: 'SupplierId', value: 'supplierId' },
                    { text: 'CategoryId', value: 'categoryId' },
                    { text: 'QuantityPerUnit', value: 'quantityPerUnit' },
                    { text: 'UnitPrice', value: 'unitPrice' },
                    { text: 'UnitsInStock', value: 'unitsInStock' },
                    { text: 'ReorderLevel', value: 'reorderLevel' },
                    { text: 'Actions', value: 'actions', sortable: false },
                ],
                editedIndex: -1,
                editedItem: {
                    id: '',
                    name: '',
                    supplierId: '',
                    categoryId: '',
                    quantityPerUnit: 0,
                    unitPrice: 0,
                    unitsInStock: 0,
                    reorderLevel: 0,

                },
                defaultItem: {
                    id: '',
                    name: '',
                    supplierId: '',
                    categoryId: '',
                    quantityPerUnit: 0,
                    unitPrice: 0,
                    unitsInStock: 0,
                    reorderLevel: 0,
                },
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Product' : 'Edit Product'
            },
        },
        watch: {
            dialog(val) {
                val
            },
        },
        methods: {
            currencyValidate($event) {
                // console.log($event.keyCode); //keyCodes value
                let keyCode = ($event.keyCode ? $event.keyCode : $event.which);

                // only allow number and one dot
                if ((keyCode < 48 || keyCode > 57) && (keyCode !== 46 || this.unitPrice.indexOf('.') != -1)) { // 46 is dot
                    $event.preventDefault();
                }

                // restrict to 2 decimal places
                if (this.unitPrice != null && this.unitPrice.indexOf(".") > -1 && (this.unitPrice.split('.')[1].length > 1)) {
                    $event.preventDefault();
                }
            },
            customFilter(items, filters, filter, headers) {
                // Init the filter class.
                const cf = new this.$MultiFilters(items, filters, filter, headers);

                cf.registerFilter('search', function (searchWord, items) {
                    if (searchWord.trim() === '') return items;

                    return items.filter(item => {
                        return item.name.toLowerCase().includes(searchWord.toLowerCase());
                    }, searchWord);

                });

                cf.registerFilter('category', function (name, items) {
                    if (name.trim() === '') return items;

                    return items.filter(item => {
                        return item.categoryId.toLowerCase() === name.toLowerCase();
                    }, name);

                });

                cf.registerFilter('supplier', function (name, items) {
                    if (name.trim() === '') return items;

                    return items.filter(item => {
                        return item.supplierId.toLowerCase() === name.toLowerCase();
                    }, name);

                });

                // Its time to run all created filters.
                // Will be executed in the order thay were defined.
                return cf.runFilters();
            },
            /**
   * Handler when user input something at the "Filter" text field.
   */
            filterSearch(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { search: val });
            },

            /**
             * Handler when user  select some category at the "Categories" select.
             */
            filterCategories(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { category: val });
            },
            /**
       * Handler when user  select some supplier at the "Suppliers" select.
       */
            filterSuppliers(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { supplier: val });
            },
            search: function () {
                let self = this;
                this.$axios.get('/product/search')
                    .then(function (response) {
                        self.products = response.data.content
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            getCategories: function () {
                let self = this;
                this.$axios.get('/category/search')
                    .then(function (response) {
                        self.categories = response.data.content
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            getSuppliers: function () {
                let self = this;
                this.$axios.get('/supplier/search')
                    .then(function (response) {
                        self.suppliers = response.data.content
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            editItem(item) {
                this.editedIndex = this.products.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },

            deleteItem(item) {
                const index = this.products.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.products.splice(index, 1)
            },

            close() {
                this.dialog = false
                this.$nextTick(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                })
            },

            save() {
                let self = this;
                self.modelstate = {}

                var formData = new FormData();

                $.each(self.editedItem, function (key, value) {
                    formData.append(key, value);
                })

                if (this.editedIndex > -1) {
                    this.$axios({
                        method: 'post',
                        url: '/product/edit',
                        headers: {
                            'content-type': 'multipart/form-data',
                            'Accept': 'application/json, application/xml, text/plain, text/html, *.*',
                        },
                        data: formData
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            Object.assign(self.products[self.editedIndex], response.data.result.content)
                            self.close();
                        }
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                } else {
                    console.log(JSON.stringify(self.editedItem));
                    console.log(formData);
                    this.$axios({
                        method: 'post',
                        url: '/product/Add',
                        headers: {
                            'content-type': 'multipart/form-data',
                            'Accept': 'application/json, application/xml, text/plain, text/html, *.*',
                        },
                        data: formData
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            self.products.push(response.data.result.content)
                            self.close();
                        }
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                }
            }
        },
        mounted() {
            this.search();
            this.getCategories();
            this.getSuppliers();
        }
    };
</script>
