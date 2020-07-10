<template>
    <div id="supplier">
        <v-app id="inspire">
            <v-data-table :headers="headers"
                          :items="suppliers"
                          sort-by="name"
                          class="elevation-1">
                <template v-slot:top>
                    <v-toolbar flat color="white">
                        <v-toolbar-title>Suppliers</v-toolbar-title>
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
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.name" label="Supplier Name" :error-messages="modelstate['Name']"></v-text-field>
                                            </v-col>

                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.contactName" label="Contact Name" :error-messages="modelstate['ContactName']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-select v-model="editedItem.contactTitle"
                                                          :items="contactTitles"
                                                          :error-messages="modelstate['ContactTitle']"
                                                          label="Contact Title"></v-select>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="12">
                                                <v-text-field v-model="editedItem.address" label="Address" :error-messages="modelstate['Address']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.city" label="City" :error-messages="modelstate['City']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.region" label="Region" :error-messages="modelstate['Region']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.postalCode" label="Postal Code" :error-messages="modelstate['PostalCode']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.country" label="Country" :error-messages="modelstate['Country']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.phone" label="Phone" :error-messages="modelstate['Phone']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="6" md="4">
                                                <v-text-field v-model="editedItem.fax" label="Fax" :error-messages="modelstate['Fax']"></v-text-field>
                                            </v-col>
                                            <v-col cols="12" sm="12" md="12">
                                                <v-text-field v-model="editedItem.website" label="Website" :error-messages="modelstate['Website']"></v-text-field>
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
        name: "supplier-component",
        data() {
            return {
                suppliers: [],
                modelstate: {},
                contactTitles: ['Mr', 'Mrs', 'Miss', 'Ms', 'Dr', 'Sir'],
                dialog: false,
                headers: [
                    {
                        text: 'Id',
                        align: 'start',
                        sortable: false,
                        value: 'id',
                    },
                    { text: 'Name', value: 'name' },
                    { text: 'ContactName', value: 'contactName' },
                    { text: 'ContactTitle', value: 'contactTitle' },
                    { text: 'Address', value: 'address' },
                    { text: 'City', value: 'city' },
                    { text: 'PostalCode', value: 'postalCode' },
                    { text: 'Country', value: 'country' },
                    { text: 'Phone', value: 'phone' },
                    { text: 'Fax', value: 'fax' },
                    { text: 'Website', value: 'website' },
                    { text: 'Actions', value: 'actions', sortable: false },
                ],
                editedIndex: -1,
                editedItem: {
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
                defaultItem: {
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
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
        },
        watch: {
            dialog(val) {
                val
            },
        },
        methods: {
            search: function () {
                let self = this;
                this.$axios.get('/supplier/search')
                    .then(function (response) {
                        console.log(response.data.content);
                        self.suppliers = response.data.content
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            editItem(item) {
                this.editedIndex = this.supplier.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },

            deleteItem(item) {
                const index = this.supplier.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.supplier.splice(index, 1)
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
                if (this.editedIndex > -1) {
                    this.$axios({
                        method: 'post',
                        url: '/supplier/edit',
                        headers: { 'Content-Type': 'application/json' },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        Object.assign(self.supplier[self.editedIndex], response.data.result.content)
                        self.close();
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                } else {
                    this.$axios({
                        method: 'post',
                        url: '/supplier/Add',
                        headers: { 'Content-Type': 'application/json' },
                        data: JSON.stringify(self.editedItem)
                    }).then((response) => {
                        self.suppliers.push(response.data.result.content);
                        self.close();
                    }, (error) => {
                        if (error.response.status == 400) {
                            self.modelstate = error.response.data;
                        }
                    });
                }
            }
        },
        mounted() {
            this.search()
        }
    };
</script>

