class $$Nav {
    init() {
        $dn.clear();
        $dn.MakeButton("Operaciones con Matrices", () => {
            Home();
            $m.crearInterfazUsuario();
        });
    }
}

const $n = new $$Nav();
