<template>
    <div id="product">
        <v-app id="inspire">
            <v-layout row wrap>
                <v-col cols="12" sm="12" md="12">
                    <v-text-field append-icon="search"
                                  label="Filter"
                                  single-line
                                  hide-details
                                  @input="filterSearch"></v-text-field>
                </v-col>
                <v-col cols="12" sm="12" md="6">
                    <v-select :items="categories"
                              item-text="name"
                              item-value="id"
                              label="Categories"
                              @change="filterCategories"></v-select>
                </v-col>
                <v-col cols="12" sm="12" md="6">
                    <v-select :items="suppliers"
                              item-text="name"
                              item-value="id"
                              label="Suppliers"
                              @change="filterSuppliers"></v-select>
                </v-col>

                <v-col cols="12" sm="12" md="12">
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
                                                        <v-text-field prefix="ZAR" v-model="editedItem.unitPrice" type="number" label="Unit Price" :error-messages="modelstate['UnitPrice']"></v-text-field>
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

                        <template v-slot:item.indicators="{ item }">
                            <v-row justify="space-around">

                                <div v-if="!item.discontinued">
                                    <v-tooltip left>
                                        <template v-slot:activator="{ on }">
                                            <v-avatar size="32" color="green" v-on="on">
                                                <v-icon dark>mdi-lock-open</v-icon>
                                            </v-avatar>
                                        </template>
                                        <span>Product still available</span>
                                    </v-tooltip>
                                </div>
                                <div v-else>
                                    <v-tooltip left>
                                        <template v-slot:activator="{ on }">
                                            <v-avatar size="32" color="red" v-on="on">
                                                <v-icon dark>mdi-lock</v-icon>
                                            </v-avatar>
                                        </template>
                                        <span>Product is not available</span>
                                    </v-tooltip>
                                </div>

                                <div v-if="item.unitsInStock > item.reorderLevel">
                                    <v-tooltip bottom>
                                        <template v-slot:activator="{ on }">
                                            <v-avatar size="32" color="green" v-on="on">
                                                <v-icon dark>mdi-store</v-icon>
                                            </v-avatar>
                                        </template>
                                        <span>Stock still good</span>
                                    </v-tooltip>
                                </div>
                                <div v-else-if="item.unitsInStock == item.reorderLevel">
                                    <v-tooltip bottom>
                                        <template v-slot:activator="{ on }">
                                            <v-avatar size="32" color="orange" v-on="on">
                                                <v-icon dark>mdi-store</v-icon>
                                            </v-avatar>
                                        </template>
                                        <span>Stock is low</span>
                                    </v-tooltip>
                                </div>
                                <div v-else>
                                    <v-tooltip bottom>
                                        <template v-slot:activator="{ on }">
                                            <v-avatar size="32" color="red" v-on="on">
                                                <v-icon dark>mdi-store</v-icon>
                                            </v-avatar>
                                        </template>
                                        <span>Stock is very low</span>
                                    </v-tooltip>
                                </div>

                            </v-row>
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
                </v-col>
            </v-layout>
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
                allProducts:[],
                categories: [],
                suppliers: [],
                defaultCategory: {
                    id: '',
                    name: '',
                    description: '',
                    imageUrl: '',
                },
                defaultSupplier: {
                    id: '',
                    name: '',
                    contactName: '',
                    contactTitle: '',
                    address: '',
                    city: '',
                    postalCode: '',
                    country: '',
                    phone: '',
                    fax: '',
                    website: '',
                },
                modelstate: {},
                dialog: false,
                headers: [
                    { text: 'Indicators', value: 'indicators', sortable: false },
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
            customFilter(items, filters, filter, headers) {
                console.log("customFilter");
                // Init the filter class.
                const cf = new this.$MultiFilters(items, filters, filter, headers);

                var searchItems = [];
                var categoryItems = [];
                var supplierItems = [];

                cf.registerFilter('search', function (searchWord, items) {
                    if (searchWord.trim() == '') return items;

                    return items.filter(item => {
                        return item.name.toLowerCase().includes(searchWord.toLowerCase());
                    }, searchWord);

                });

                cf.registerFilter('category', function (name, items) {
                    if (name.trim() == '') return items;

                    return items.filter(item => {
                        return item.categoryId.toLowerCase() === name.toLowerCase();
                    }, name);

                });

                cf.registerFilter('supplier', function (name, items) {
                    if (name.trim() == '') return items;

                    return items.filter(item => {
                        return item.supplierId.toLowerCase() === name.toLowerCase();
                    }, name);

                });

                // Its time to run all created filters.
                // Will be executed in the order thay were defined.
                this.products = cf.runFilters();
            },
            /**
   * Handler when user input something at the "Filter" text field.
   */
            filterSearch(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { search: val });

                this.customFilter(this.allProducts, this.filters, val, this.headers);
            },

            /**
             * Handler when user  select some category at the "Categories" select.
             */
            filterCategories(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { category: val });

                this.customFilter(this.allProducts, this.filters, val, this.headers);
            },
            /**
       * Handler when user  select some supplier at the "Suppliers" select.
       */
            filterSuppliers(val) {
                this.filters = this.$MultiFilters.updateFilters(this.filters, { supplier: val });

                this.customFilter(this.allProducts, this.filters, val, this.headers);
            },
            search: function () {
                let self = this;
                this.$axios.get('/product/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.products = response.data.content;

                            self.allProducts = self.products;
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            getCategories: function () {
                let self = this;
                this.$axios.get('/category/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.categories = response.data.content
                            self.categories.push(self.defaultCategory);

                            self.categories = self.categories.sort((a, b) => (a.name > b.name) ? 1 : -1);
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            getSuppliers: function () {
                let self = this;
                this.$axios.get('/supplier/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.suppliers = response.data.content
                            self.suppliers.push(self.defaultSupplier);

                            self.suppliers = self.suppliers.sort((a, b) => (a.name > b.name) ? 1 : -1);
                        }
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
                    this.$axios({
                        method: 'post',
                        url: '/product/Add',
                        headers: {
                            'content-type': 'multipart/form-data',
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
