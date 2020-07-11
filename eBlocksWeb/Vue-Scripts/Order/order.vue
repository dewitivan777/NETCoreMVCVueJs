<template>
    <div id="order">
        <v-app id="inspire">
            <v-data-table :headers="headers"
                          :items="orders"
                          sort-by="name"
                          class="elevation-1">
                <template v-slot:top>
                    <v-toolbar flat color="white">
                        <v-toolbar-title>Orders</v-toolbar-title>
                        <v-divider class="mx-4"
                                   inset
                                   vertical></v-divider>
                        <v-spacer></v-spacer>
                        <!--Create Dialog-->
                        <v-dialog v-model="createDialog" max-width="500px">
                            <template v-slot:activator="{ on, attrs }">
                                <v-btn color="primary"
                                       dark
                                       class="mb-2"
                                       v-bind="attrs"
                                       v-on="on">Create New Order</v-btn>
                            </template>
                            <v-card>
                                <v-card-title>
                                    <span class="headline">{{ formTitle }}</span>
                                </v-card-title>

                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <v-col cols="12" sm="12" md="12">
                                                <v-select v-model="selectedProduct"
                                                          :items="products"
                                                          return-object
                                                          item-text="name"
                                                          :search-input.sync="searchProduct"
                                                          :error-messages="modelstate['ProductId']"
                                                          autocomplete
                                                          label="Product"></v-select>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="12">
                                                <p>   Unit Price: R{{ editedItem.unitPrice }} </p>

                                            </v-col>
                                            <v-col cols="12" sm="6" md="6">
                                                <v-text-field v-model="editedItem.quantity" type="number" min="0" label="Quantity" :error-messages="modelstate['Quantity']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="6">
                                                <v-text-field prefix="ZAR" v-model="editedItem.discount" min="0" type="number" label="Discount" :error-messages="modelstate['Discount']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="12">
                                                <h3>Total : R{{ computedTotal }}</h3>
                                            </v-col>
                                        </v-row>
                                    </v-container>
                                </v-card-text>

                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="closeCreate">Cancel</v-btn>
                                    <v-btn color="blue darken-1" text @click="save">Save</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>

                        <!--Edit Dialog-->
                        <v-dialog v-model="editDialog" max-width="500px">
                            <v-card>
                                <v-card-title>
                                    <span class="headline">{{ formTitle }}</span>
                                </v-card-title>

                                <v-card-text>
                                    <v-container>
                                        <v-row>
                                            <div v-if="">

                                            </div>
                                            <div v-else>

                                            </div>
                                        </v-row>
                                    </v-container>
                                </v-card-text>

                                <v-card-actions>
                                    <v-spacer></v-spacer>
                                    <v-btn color="blue darken-1" text @click="closeEdit">Cancel</v-btn>
                                    <v-btn color="blue darken-1" text @click="save">Save</v-btn>
                                </v-card-actions>
                            </v-card>
                        </v-dialog>

                    </v-toolbar>
                </template>

                <template v-slot:item.indicators="{ item }">
                    <v-row justify="space-around">

                        <div v-if="item.state == 'Completed'">
                            <v-tooltip bottom>
                                <template v-slot:activator="{ on }">
                                    <v-avatar size="32" color="green" v-on="on">
                                    </v-avatar>
                                </template>
                                <span>Order Completed</span>
                            </v-tooltip>
                        </div>
                        <div v-if="item.state == 'Processing'">
                            <v-tooltip bottom>
                                <template v-slot:activator="{ on }">
                                    <v-avatar size="32" color="orange" v-on="on">
                                    </v-avatar>
                                </template>
                                <span>Order Processing</span>
                            </v-tooltip>
                        </div>
                        <div v-else>
                            <v-tooltip bottom>
                                <template v-slot:activator="{ on }">
                                    <v-avatar size="32" color="red" v-on="on">
                                    </v-avatar>
                                </template>
                                <span>Order Canceled</span>
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
        name: "order-component",
        data() {
            return {
                orders: [],
                products: [],
                selectedProduct: {
                    id: '',
                    name: '',
                    supplierId: '',
                    categoryId: '',
                    quantityPerUnit: 0,
                    unitPrice: 0,
                    unitsInStock: 0,
                    reorderLevel: 0,
                    discontinued: false,
                },
                states: ['Completed', 'Processing', 'Canceled'],
                modelstate: {},
                createDialog: false,
                editDialog: false,
                searchProduct: "",
                headers: [
                    { text: 'Indicators', value: 'indicators', sortable: false },
                    {
                        text: 'Id',
                        align: 'start',
                        sortable: false,
                        value: 'id',
                    },
                    { text: 'CreatedDate', value: 'createdDate' },
                    { text: 'UpdateDate', value: 'updateDate' },
                    { text: 'ProductId', value: 'productId' },
                    { text: 'UnitPrice', value: 'unitPrice' },
                    { text: 'Quantity', value: 'quantity' },
                    { text: 'Discount', value: 'discount' },
                    { text: 'Price', value: 'price' },
                    { text: 'Actions', value: 'actions', sortable: false },
                ],
                editedIndex: -1,
                editedItem: {
                    id: '',
                    createdDate: '',
                    updateDate: '',
                    productId: '',
                    unitPrice: 0,
                    quantity: 0,
                    discount: 0,
                    price: 0,
                    State: '',
                },
                defaultItem: {
                    id: '',
                    createdDate: '',
                    updateDate: '',
                    productId: '',
                    unitPrice: 0,
                    quantity: 0,
                    discount: 0,
                    price: 0,
                    State: '',
                },
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Order' : 'Update Order'
            },
            computedTotal() {
                if (this.selectedProduct.id.length) {
                    var total = (this.editedItem.unitPrice * this.editedItem.quantity) - this.editedItem.discount;
                    return total;
                }
            },
        },
        watch: {
            createDialog(val) {
                val
            },
            editDialog(val) {
                val
            },
            selectedProduct: function (val) {
                this.editedItem.unitPrice = val.unitPrice;
                this.editedItem.productId = val.id;
            }
        },
        methods: {
            search: function () {
                let self = this;
                this.$axios.get('/order/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.orders = response.data.content
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            getProducts: function () {
                let self = this;
                this.$axios.get('/product/search')
                    .then(function (response) {
                        if (response.data.content != null) {
                            self.products = response.data.content;
                            self.products.push(self.selectedProduct);

                            self.products = self.products.sort((a, b) => (a.name > b.name) ? 1 : -1);
                        }
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            editItem(item) {
                this.editedIndex = this.orders.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.editDialog = true
            },

            deleteItem(item) {
                const index = this.orders.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.categories.splice(index, 1)
            },

            closeEdit() {
                this.editDialog = false;
                this.$nextTick(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                })
            },
            closeCreate() {
                this.createDialog = false;
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
                        url: '/order/edit',
                        headers: {
                            'content-type': 'multipart/form-data',
                        },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            Object.assign(self.orders[self.editedIndex], response.data.result.content)
                            self.selectedProduct = self.products[0];
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
                        url: '/order/Add',
                        headers: {
                            'content-type': 'multipart/form-data',
                        },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        if (!response.data.result.isError) {
                            if (response.data.result.content) { }
                            self.orders.push(response.data.result.content)
                            self.selectedProduct = self.products[0];
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
            this.getProducts();
            this.search();
        }
    };
</script>
